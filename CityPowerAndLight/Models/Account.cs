using System.Text.Json.Serialization;

namespace CityPowerAndLight.Models
{
    internal class Account : Model
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("telephone1")]
        public string Phone { get; set; }
        [JsonPropertyName("address1_city")]
        public string City { get; set; }
        [JsonPropertyName("accountid")]
        public string Id { get; set; }

        public Account(string name, string phone, string city)
        {
            Name = name;
            Phone = phone;
            City = city;
        }

        public string GetPayload()
        {
            return $"{{name: \"{Name}\", telephone1: \"{Phone}\", address1_city: \"{City}\"}}";
        }
    }
}
