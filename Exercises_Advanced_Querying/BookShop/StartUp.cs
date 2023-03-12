namespace BookShop
{
    using Castle.Core.Internal;
    using Data;
    using Initializer;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            DbInitializer.ResetDatabase(context);


            string input = Console.ReadLine();
            Console.Write(GetBooksByAgeRestriction(context, input));

        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var bookTitles = context.Books
                .ToArray()
                .Where(b => !command.IsNullOrEmpty() &&
                b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);

        }


    }
}


