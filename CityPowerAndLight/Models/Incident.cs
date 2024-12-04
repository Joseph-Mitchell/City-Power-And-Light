using System.Text.Json.Serialization;

namespace CityPowerAndLight.Models
{
    internal class Incident
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("_customerid_value")]
        public string CustomerId { get; set; }
        [JsonPropertyName("incidentid")]
        public string Id { get; set; }

        public Incident(string title, string description, string customerId)
        {
            Title = title;
            Description = description;
            CustomerId = customerId;
        }
    }
}
