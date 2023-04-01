namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using AutoMapper;
    using CarDealer.Utulities;
    using Castle.Core.Internal;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utulities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";


        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var mapper = MyMapper.CreateMapper();
            var xmlHelper = new XmlHelper();

            var despatcherDtos = xmlHelper.Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");
            var validDespatchers = new HashSet<Despatcher>();

            foreach (var despDto in despatcherDtos)
            {
                if (!IsValid(despDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var validTrucks = new HashSet<Truck>();
                foreach (var truckDto in despDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validTrucks.Add(mapper.Map<Truck>(truckDto));
                }
                var despatcher = mapper.Map<Despatcher>(despDto);
                despatcher.Trucks = validTrucks;
                validDespatchers.Add(despatcher);
                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
            }
            context.Despatchers.AddRange(validDespatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }



        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var mapper = MyMapper.CreateMapper();
            var clientDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);
            var validClients = new HashSet<Client>();
            foreach (var clientDto in clientDtos!)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var existingTrucksIDs = context.Trucks.Select(t => t.Id).ToArray();
                var client = mapper.Map<ImportClientDto, Client>(clientDto);

                foreach (var truckId in clientDto.Trucks.Distinct())
                {
                    if (!existingTrucksIDs.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    client.ClientsTrucks.Add(new ClientTruck { ClientId = client.Id, TruckId = truckId });
                }

                validClients.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }
            context.Clients.AddRange(validClients);
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