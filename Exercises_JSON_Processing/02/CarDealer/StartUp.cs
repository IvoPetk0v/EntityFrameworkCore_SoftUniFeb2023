using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Runtime.InteropServices;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            string input = File.ReadAllText("../../../Datasets/customers.json");

            var context = new CarDealerContext();

            // Console.WriteLine(ImportSuppliers(context, input));
            // Console.WriteLine(ImportParts(context, input));
            // Console.WriteLine(ImportCars(context, input));
            Console.WriteLine(ImportCustomers(context,input));

        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<CarDealerProfile>()));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();

            var supplyerDtos = JsonConvert.DeserializeObject<ImportSupplyerDto[]>(inputJson);

            var validSuppliers = new HashSet<Supplier>();

            foreach (var s in supplyerDtos!)
            {
                var supplyer = mapper.Map<Supplier>(s);
                validSuppliers.Add(supplyer);
            }

            context.AddRange(validSuppliers);
            context.SaveChanges();


            return $"Successfully imported {validSuppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            //If the supplierId doesn't exist in the Suppliers table, skip the record.
            var validSuppliers = context.Suppliers
                .Select(s => s.Id)
                .ToArray();

            var mapper = CreateMapper();
            var partDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

            var validParts = new HashSet<Part>();

            foreach (var p in partDtos!)
            {
                if (validSuppliers.Contains(p.SupplierId))
                {
                    var part = mapper.Map<Part>(p);
                    validParts.Add(part);
                }
            }
            context.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            var validCars = new HashSet<Car>();
            var validParts = new HashSet<PartCar>();
            var carDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);

            foreach (var carDto in carDtos!)
            {
                var car = mapper.Map<Car>(carDto);

                validCars.Add(car);
                foreach (var partId in carDto.PartsId)
                {
                    var part = new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    };
                    validParts.Add(part);
                }
            }
            context.AddRange(validCars);
            context.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}.";

        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            var validCustomers = new HashSet<Customer>();
            var customerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);

            foreach (var c in customerDtos!)
            {
                validCustomers.Add(mapper.Map<Customer>(c));
            }

            context.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count}.";
        }

    }
}
