using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.InteropServices;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            string input = File.ReadAllText("../../../Datasets/suppliers.json");

            var context = new CarDealerContext();

            Console.WriteLine(ImportSuppliers(context, input));
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

    }

}