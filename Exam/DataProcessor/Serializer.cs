namespace Boardgames.DataProcessor
{
    using AutoMapper.QueryableExtensions;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ExportDto;
    using CarDealer.Utulities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Text;
    using Trucks.Utulities;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var xmlHelper = new XmlHelper();
            var sb = new StringBuilder();

            var creators = context.Creators
                .Where(c => c.Boardgames.Any())
                .ProjectTo<ExportCreatorDto>(MyMapper.CreateMapper().ConfigurationProvider)
                .ToArray();

            creators = creators
                 .OrderByDescending(c => c.BoardgamesCount)
                 .ThenBy(c => c.CreatorName).ToArray();
            return xmlHelper.Serialize(creators, "Creators");

        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Count >= 1 &&
                s.BoardgamesSellers.Any(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating))
                               .ToArray()
          .Select(s => new
          {
              s.Name,
              s.Website,
              Boardgames = s.BoardgamesSellers.Where(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating)
                              .Select(bs => new
                              {
                                  bs.Boardgame.Name,
                                  bs.Boardgame.Rating,
                                  bs.Boardgame.Mechanics,
                                  Category = bs.Boardgame.CategoryType.ToString()

                              })
                    .OrderByDescending(bs => bs.Rating)
                    .ThenBy(bs => bs.Name)
                    .ToArray()
          })
                                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                                 .Take(5)
                .ToArray();
            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}