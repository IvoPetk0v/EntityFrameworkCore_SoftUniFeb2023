﻿using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Internal;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            //Console.WriteLine(ImportSuppliers(context,
            //      File.ReadAllText("../../../Datasets/suppliers.json")));

            //Console.WriteLine(ImportParts(context,
            //      File.ReadAllText("../../../Datasets/parts.json")));

            //Console.WriteLine(ImportCars(context,
            //      File.ReadAllText("../../../Datasets/cars.json")));

            //Console.WriteLine(ImportCustomers(context,
            //      File.ReadAllText("../../../Datasets/customers.json")));

            //Console.WriteLine(ImportSales(context,
            //      File.ReadAllText("../../../Datasets/sales.json")));

            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }
        //Import data methods
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

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            var validSales = new HashSet<Sale>();
            var saleDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

            foreach (var s in saleDtos!)
            {
                validSales.Add(mapper.Map<Sale>(s));
            }

            context.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count}.";
        }

        // Export data methods
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString(("dd/MM/yyyy")),
                    IsYoungDriver = c.IsYoungDriver
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {

            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                })
                .AsNoTracking()
                .ToArray();
            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.IsNullOrEmpty() ? 0 : s.Parts.Count(),
                })
                .AsNoTracking()
                .ToArray();
            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .OrderBy(c => c.Id)
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },
                    parts = c.PartsCars
                    .Select(p => new
                    {
                        p.Part.Name,
                        Price = p.Part.Price.ToString("f2")
                    }).ToArray()

                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales
                        .SelectMany(s => s.Car.PartsCars)
                        .Sum(p => p.Part.Price)
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                        {
                            s.Car.Make,
                            s.Car.Model,
                            s.Car.TraveledDistance
                        },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Sum(p => p.Part.Price).ToString("f2"),
                    priceWithDiscount = (s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - s.Discount / 100)).ToString("f2")
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);

        }
    }
}
