using System.Text.Json.Serialization;

namespace CityPowerAndLight.Models
{
    internal class Account : Model
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("accountid")]
        public string Id { get; set; }

        public Account(string name)
        {
            Name = name;
        }

        public string GetPayload()
        {
            return $"{{name: \"{Name}\"}}";
        }
    }
}
