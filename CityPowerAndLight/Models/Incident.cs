using System.Text.Json.Serialization;

namespace CityPowerAndLight.Models
{
    internal enum Priority
    {
        High = 1,
        Normal = 2,
        Low = 3
    }

    internal class Incident : Model
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("prioritycode")]
        public Priority Priority { get; set; }
        [JsonPropertyName("_customerid_value")]
        public string CustomerId { get; set; }
        [JsonPropertyName("incidentid")]
        public string Id { get; set; }

        public Incident(string title, string description, Priority priority, string customerId)
        {
            Title = title;
            Description = description;
            Priority = priority;
            CustomerId = customerId;
        }

        public string GetPayload()
        {
            return $"{{title: \"{Title}\",description: \"{Description}\",prioritycode: \"{(int)Priority}\",\"customerid_contact@odata.bind\": \"contacts({CustomerId})\"}}";
        }

        public static string GeneratePayload(string title, string description, Priority priority, string customerId)
        {
            return $"{{title: \"{title}\",description: \"{description}\",prioritycode: \"{(int)priority}\",\"customerid_contact@odata.bind\": \"contacts({customerId})\"}}";
        }
    }
}
