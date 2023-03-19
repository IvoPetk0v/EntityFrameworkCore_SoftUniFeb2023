namespace ProductShop
{
    using AutoMapper;
    using Castle.Core.Internal;
    using Newtonsoft.Json;

    using Data;
    using DTOs.Import;
    using Models;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using ProductShop.DTOs.Export;

    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            // string input = File.ReadAllText(@"../../../Datasets/categories-products.json");

            string result = GetSoldProducts(context);

            Console.WriteLine(result);
        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var mapper = CreateMapper();
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);
            var validUsers = new HashSet<User>();

            foreach (var userDto in userDtos!)
            {
                var user = mapper.Map<User>(userDto);
                validUsers.Add(user);
            }

            context.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var mapper = CreateMapper();
            var productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);
            Product[] validProducts = mapper.Map<Product[]>(productDtos);
            context.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var mapper = CreateMapper();

            var categoriesDtos = JsonConvert.DeserializeObject<Category[]>(inputJson);
            var validCategories = new HashSet<Category>();

            foreach (var category in categoriesDtos!)
            {
                if (category.Name.IsNullOrEmpty())
                {
                    continue;
                }
                validCategories.Add(mapper.Map<Category>(category));
            }
            context.AddRange(validCategories);
            context.SaveChanges();
            return $"Successfully imported {validCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var mapper = CreateMapper();
            var categoryProductDtos = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            CategoryProduct[] categoryProducts = mapper.Map<CategoryProduct[]>(categoryProductDtos);

            context.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var mapper = CreateMapper();

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .AsNoTracking()
                .ProjectTo<ProductInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var mapper = CreateMapper();

            var users = context.Users
                .Where(u => u.ProductsSold.Any(p=>p.BuyerId.HasValue))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .AsNoTracking()
                .ProjectTo<UserSoldProductDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }
    }
}