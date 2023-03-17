namespace BookShop
{
    using Microsoft.EntityFrameworkCore;
    using System.Text;
    using System.Globalization;
    using System.Linq;
    using System.Collections.Immutable;

    using Models.Enums;
    using Castle.Core.Internal;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(GetMostRecentBooks(context));

            //IncreasePrices(context);
            Console.WriteLine(RemoveBooks(context));

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

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => b.Title + $" ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var bookCount = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray()
                .Length;

            return bookCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                          .Select(a => new
                          {
                              a.FirstName,
                              a.LastName,
                              BookCopies = a.Books.Sum(b => b.Copies)
                          })
              .OrderByDescending(b => b.BookCopies)
              .ToArray();

            var sb = new StringBuilder();

            foreach (var a in authors)
            {
                sb.AppendLine($"{a.FirstName} {a.LastName} - {a.BookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {

            var categories = context.Categories
                 .Select(c => new
                 {
                     CategoryName = c.Name,
                     TotalProfit = c.CategoryBooks
                       .Sum(cb => cb.Book.Copies * cb.Book.Price)
                 })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.CategoryName} ${category.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {

            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks
                         .Select(cb => cb.Book)
                         .OrderByDescending(b => b.ReleaseDate)
                         .Take(3)
                         .ToArray()
                })
                .OrderBy(c => c.CategoryName)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var c in categories)
            {
                sb.AppendLine($"--{c.CategoryName}");

                foreach (var b in c.MostRecentBooks)
                {
                    sb.AppendLine($"{b.Title} ({b.ReleaseDate!.Value.Year})");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {

            var books = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010);
            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();

        }

        public static int RemoveBooks(BookShopContext context)
        {
         
            var booksForRemoving = context.Books.Where(b => b.Copies < 4200);
            int count = booksForRemoving.Count();
            
            context.RemoveRange(booksForRemoving);
            context.SaveChanges();

            return count;
        }
    }
}


