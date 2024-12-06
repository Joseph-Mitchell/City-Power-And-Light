using System.Text.Json.Serialization;

namespace CityPowerAndLight.Models
{
    internal class Contact : Model
    {
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastname")]
        public string Lastname { get; set; }
        [JsonPropertyName("emailaddress1")]
        public string Email { get; set; }
        [JsonPropertyName("telephone1")]
        public string Phone { get; set; }
        [JsonPropertyName("_parentcustomerid_value")]
        public string AccountId { get; set; }
        [JsonPropertyName("contactid")]
        public string Id { get; set; }

        public Contact(string firstName, string lastName, string email, string phone, string accountId)
        {
            FirstName = firstName;
            Lastname = lastName;
            Email = email;
            Phone = phone;
            AccountId = accountId;
        }

        public string GetPayload()
        {
            return $"{{firstname: \"{FirstName}\",lastname: \"{Lastname}\",emailaddress1: \"{Email}\",telephone1: \"{Phone}\",\"parentcustomerid_account@odata.bind\": \"accounts({AccountId})\"}}";
        }
    }
}
