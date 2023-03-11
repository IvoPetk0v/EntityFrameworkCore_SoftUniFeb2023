namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here


            string result = ExportAlbumsInfo(context, 9);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var sb = new StringBuilder();

            var albums = context.Albums
                 .Where(a => a.ProducerId.HasValue
                        && a.ProducerId.Value == producerId)
                 .ToArray()
                 .OrderByDescending(a => a.Price)
                 .Select(a => new
                 {
                     a.Name,
                     ReleaseDate = a.ReleaseDate
                     .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                     ProducerName = a.Producer.Name,
                     Songs = a.Songs
                      .Select(s => new
                      {
                          s.Name,
                          Price = s.Price.ToString("f2"),
                          SongWriterName = s.Writer.Name

                      })
                      .OrderByDescending(s => s.Name)
                      .ThenBy(s => s.SongWriterName)
                      .ToArray(),
                     TotalAlbumPrice = a.Price.ToString("f2")
                 }).ToArray();

            foreach (var album in albums)
            {
                sb
              .AppendLine($"-AlbumName: {album.Name}")
              .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
              .AppendLine($"-ProducerName: {album.ProducerName}")
              .AppendLine($"-Songs:");

                int counter = 1;
                foreach (var s in album.Songs)
                {
                    sb
                         .AppendLine($"---#{counter}")
                        .AppendLine($"---SongName: {s.Name}")
                        .AppendLine($"---Price: {s.Price}")
                        .AppendLine($"---Writer: {s.SongWriterName}");
                    counter++;
                }
                sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }


    }
}
