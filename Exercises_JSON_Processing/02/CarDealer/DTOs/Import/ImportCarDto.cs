using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CarDealer.DTOs.Import
{

    public class ImportCarDto
    {
        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }

        [JsonProperty("partsId")]
        public HashSet<int> PartsId { get; set; } = new HashSet<int>();
    }
}