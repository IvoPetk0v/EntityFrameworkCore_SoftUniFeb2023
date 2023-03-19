namespace ProductShop
{
    using AutoMapper;
    using Castle.Core.Internal;
    using Newtonsoft.Json;

    using Data;
    using DTOs.Import;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            string input = File.ReadAllText(@"../../../Datasets/categories.json");

            string result = ImportCategories(context, input);

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
    }




}