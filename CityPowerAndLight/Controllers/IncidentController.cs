using CityPowerAndLight.Models;
using DotNetEnv;
using System.Text.Json;

namespace CityPowerAndLight.Controllers
{
    internal class IncidentController
    {
        private struct MultiResponse
        {
            public Incident[] value { get; set; }
        }

        private string _apiUrl;
        private HttpClient _client;

        public IncidentController(HttpClient client)
        {
            _apiUrl = Env.GetString("API_URL");
            _client = client;
        }

        public async Task<Incident> Create(string title, string desc, string contactId)
        {
            //Convert incident object to json string
            string payload = $"{{title: \"{title}\",description: \"{desc}\",\"customerid_contact@odata.bind\": \"contacts({contactId})\"}}";

            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_apiUrl + "incidents", content);

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to Incident object
            Incident incident = JsonSerializer.Deserialize<Incident>(responseString);

            return incident;
        }

        public async Task<Incident[]> ReadAll()
        {
            //Send a get request for all incidents of the customer hub
            var response = await _client.GetAsync(_apiUrl + "incidents");

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to object with an array of Incidents
            MultiResponse parsedResponse = JsonSerializer.Deserialize<MultiResponse>(responseString);

            return parsedResponse.value;
        }

        public async Task<Incident> ReadOne(string id)
        {
            //Send a get request for incident with matching id
            var response = await _client.GetAsync(_apiUrl + $"incidents({id})");

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to Incident object
            Incident incident = JsonSerializer.Deserialize<Incident>(responseString);

            return incident;
        }

        public async Task<Incident> Update(string id, string title, string description, string customerId)
        {
            //Convert Incident object to json string
            string payload = $"{{title: \"{title}\",description: \"{description}\",\"customerid_contact@odata.bind\": \"contacts({customerId})\"}}";

            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync(_apiUrl + $"incidents({id})", content);

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to Incident object
            Incident incident = JsonSerializer.Deserialize<Incident>(responseString);

            return incident;
        }

        public async Task Delete(string id)
        {
            var response = await _client.DeleteAsync(_apiUrl + $"incidents({id})");
        }
    }
}
