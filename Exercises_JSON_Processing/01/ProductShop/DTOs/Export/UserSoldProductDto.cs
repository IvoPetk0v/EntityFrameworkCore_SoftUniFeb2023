using Newtonsoft.Json;

namespace ProductShop.DTOs.Export
{
    public class UserSoldProductDto
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty("soldProducts")]
        public SoldProductDto[] SoldProducts { get; set; } = null!;


    }
}
