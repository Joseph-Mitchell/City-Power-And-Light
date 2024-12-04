using System.Text.Json.Serialization;

namespace CityPowerAndLight.Models
{
    internal class Contact
    {
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastname")]
        public string Lastname { get; set; }
        [JsonPropertyName("_parentcustomerid_value")]
        public string AccountId { get; set; }
        [JsonPropertyName("contactid")]
        public string Id { get; set; }

        public Contact(string firstName, string lastName, string accountId)
        {
            FirstName = firstName;
            Lastname = lastName;
            AccountId = accountId;
        }
    }
}
