using Newtonsoft.Json;

namespace Domain.External
{
    public class IpStackResponse
    {
        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}