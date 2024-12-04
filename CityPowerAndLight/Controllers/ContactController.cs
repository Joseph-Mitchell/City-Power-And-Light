using CityPowerAndLight.Models;
using DotNetEnv;
using System.Text.Json;

namespace CityPowerAndLight.Controllers
{
    internal class ContactController
    {
        private struct MultiResponse
        {
            public Contact[] value { get; set; }
        }

        private string _apiUrl;
        private HttpClient _client;

        public ContactController(HttpClient client)
        {
            _apiUrl = Env.GetString("API_URL");
            _client = client;
        }

        public async Task<Contact> Create(string firstName, string lastName, string accountId)
        {
            //Convert contact object to json string
            string payload = $"{{firstname: \"{firstName}\",lastname: \"{lastName}\",\"parentcustomerid_account@odata.bind\": \"accounts({accountId})\"}}";

            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_apiUrl + "contacts", content);

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to Contact object
            Contact contact = JsonSerializer.Deserialize<Contact>(responseString);

            return contact;
        }

        public async Task<Contact[]> ReadAll()
        {
            //Send a get request for all contacts of the customer hub
            var response = await _client.GetAsync(_apiUrl + "contacts");

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to object with an array of Contacts
            MultiResponse parsedResponse = JsonSerializer.Deserialize<MultiResponse>(responseString);

            return parsedResponse.value;
        }

        public async Task<Contact> ReadOne(string id)
        {
            //Send a get request for contact matching id
            var response = await _client.GetAsync(_apiUrl + $"contacts({id})");

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to Contact object
            Contact contact = JsonSerializer.Deserialize<Contact>(responseString);

            return contact;
        }

        public async Task Update(string id, Contact contact)
        {
            //Convert contact object to json string
            string payload = $"{{firstname: \"{contact.FirstName}\",lastname: \"{contact.Lastname}\"}}";

            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync(_apiUrl + $"contacts({id})", content);

            payload = $"{{\"parentcustomerid_account@odata.bind\": \"accounts({contact.AccountId})\"}}";

            content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            response = await _client.PatchAsync(_apiUrl + $"contacts({id})", content);
        }

        public async Task Delete(string id)
        {
            var response = await _client.DeleteAsync(_apiUrl + $"contacts({id})");
        }
    }
}
