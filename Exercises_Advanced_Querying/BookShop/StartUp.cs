namespace BookShop
{
    using Microsoft.EntityFrameworkCore;

    using System.Linq;
    using System.Collections.Immutable;

    using Models.Enums;
    using Castle.Core.Internal;
    using Data;
    using Initializer;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Query.Internal;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            // DbInitializer.ResetDatabase(context);

            string input = Console.ReadLine();

            Console.Write(GetBookTitlesContaining(context, input));

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

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            var books = context.Books.ToArray()
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {

            var bookCategories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => bookCategories.Contains(bc.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateFormated = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                 .Where(b => b.ReleaseDate < dateFormated)
                 .OrderByDescending(b => b.ReleaseDate)
                 .Select(b => new
                 {
                     b.Title,
                     b.EditionType,
                     b.Price
                 })
                 .ToArray();

            var sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => a.FirstName + " " + a.LastName)
                .OrderBy(a => a)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string subStr = input.ToLower();
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(subStr))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

    }
}


