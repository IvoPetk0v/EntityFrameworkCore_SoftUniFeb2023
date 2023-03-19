
using Newtonsoft.Json;

namespace ProductShop.DTOs.Export
{
    public class CategoryByCountDto
    {
        [JsonProperty("category")]
        public string Name { get; set; } = null!;

        [JsonProperty("productsCount")]
        public int Count { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public string TotalRevenue { get; set; }
    }
}
