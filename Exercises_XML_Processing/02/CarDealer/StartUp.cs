namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using CarDealer.Utulities;
    using Castle.Core.Internal;
    using Castle.Core.Resource;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.IO;
    using System.Runtime.Versioning;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new CarDealerContext();
            // Run Import methods here
            // string input = File.ReadAllText("../../../Datasets/sales.xml");
            // Console.WriteLine(ImportSales(context, input));

        }

        private static IMapper CreateMapper()
           => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>()));

        //Import data methods 
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            var supplierDtos = xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers").ToArray();
            var validSuppliers = new HashSet<Supplier>();

            foreach (var s in supplierDtos)
            {
                if (!s.Name.IsNullOrEmpty())
                {
                    var supplier = mapper.Map<Supplier>(s);
                    validSuppliers.Add(supplier);
                }
            }
            context.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            var partDtos = xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");
            var validParts = new HashSet<Part>();

            foreach (var p in partDtos)
            {
                if (!p.SupplierId.HasValue ||
                    !context.Suppliers.Any(s => s.Id == p.SupplierId) ||
                    p.Name.IsNullOrEmpty())
                {
                    continue;
                }
                var part = mapper.Map<Part>(p);
                validParts.Add(part);
            }

            context.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            ImportCarDto[] carDtos = xmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");
            var validCars = new HashSet<Car>();

            foreach (var carDto in carDtos)
            {
                if (carDto.Make.IsNullOrEmpty() || carDto.Model.IsNullOrEmpty())
                {
                    continue;
                }
                var car = mapper.Map<Car>(carDto);

                foreach (var carPartDto in carDto.Parts.DistinctBy(p => p.PartId))
                {
                    if (!context.Parts.Any(p => p.Id == carPartDto.PartId))
                    {
                        continue;
                    }
                    var carPart = new PartCar()
                    {
                        PartId = carPartDto.PartId
                    };

                    car.PartsCars.Add(carPart);
                }
                validCars.Add(car);
            }
            context.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var xmlHelper = new XmlHelper();
            var validCustomers = new HashSet<Customer>();

            var customerDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

            foreach (var c in customerDtos)
            {
                var customer = mapper.Map<Customer>(c);
                validCustomers.Add(customer);
            }

            context.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var xmlHelper = new XmlHelper();
            var validSales = new HashSet<Sale>();

            var saleDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

            ICollection<int> dbCarIds = context.Cars
               .Select(c => c.Id)
               .ToArray();

            foreach (var s in saleDtos)
            {
                if (dbCarIds.All(id => id != s.CarId))
                {
                    continue;
                }
                var sale = mapper.Map<Sale>(s);
                validSales.Add(sale);
            }

            context.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count}";
        }

    }
}