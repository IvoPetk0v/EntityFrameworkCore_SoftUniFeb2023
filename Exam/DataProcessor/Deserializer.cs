namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ImportDto;
    using CarDealer.Utulities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Trucks.Utulities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlHelper = new XmlHelper();
            var creatorDtos = xmlHelper.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");
            var validCreators = new HashSet<Creator>();
            foreach (var creatorDto in creatorDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var creator = MyMapper.CreateMapper().Map<Creator>(creatorDto);

                var validBoardgames = new HashSet<Boardgame>();
                foreach (var boardgameDto in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    validBoardgames.Add(MyMapper.CreateMapper().Map<Boardgame>(boardgameDto));
                }
                creator.Boardgames = validBoardgames;
                validCreators.Add(creator);
                sb.AppendLine(String.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }
            context.Creators.AddRange(validCreators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var sellerDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);
            var validSellers = new HashSet<Seller>();
            foreach (var sellerDto in sellerDtos!)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var seller = MyMapper.CreateMapper().Map<ImportSellerDto, Seller>(sellerDto);

                var existingBoargameIds = context.Boardgames.Select(b => b.Id).ToArray();
                foreach (var boardgameId in sellerDto.Boardgames.Distinct())
                {
                    if (!existingBoargameIds.Contains(boardgameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    seller.BoardgamesSellers
                            .Add(new BoardgameSeller
                            {
                                BoardgameId = boardgameId,
                                SellerId = seller.Id
                            });
                }
                validSellers.Add(seller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }
            context.AddRange(validSellers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
