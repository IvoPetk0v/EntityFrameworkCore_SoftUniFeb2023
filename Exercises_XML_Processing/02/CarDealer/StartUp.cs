namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using CarDealer.Utulities;
    using Castle.Core.Internal;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.IO;
    using System.Runtime.Versioning;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new CarDealerContext();

            string input = File.ReadAllText("../../../Datasets/parts.xml");


            Console.WriteLine(ImportParts(context, input));

        }
        private static IMapper CreateMapper()
           => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>()));


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

    }
}