namespace BookShop
{
    using System.Linq;

    using Models.Enums;
    using Castle.Core.Internal;
    using Data;
    using Initializer;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            DbInitializer.ResetDatabase(context);


            Console.Write(GetBooksByPrice(context));

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

        public static string GetGoldenBooks(BookShopContext context)
        {

            var booksTitle = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            return string.Join(Environment.NewLine, booksTitle);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            //Return in a single string all titles and prices of books with a price higher than 40, each on a new row in the format given below. Order them by price descending...

            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new { b.Title, b.Price })
                .ToArray();
            var sb = new StringBuilder();
            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - ${b.Price:f2}");
            }
            return sb.ToString().TrimEnd();
        }


    }
}


