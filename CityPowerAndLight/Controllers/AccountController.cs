using CityPowerAndLight.Models;
using DotNetEnv;
using System.Text.Json;

namespace CityPowerAndLight.Controllers
{
    internal class AccountController
    {
        private struct MultiResponse
        {
            public Account[] value { get; set; }
        }

        private string _apiUrl;
        private HttpClient _client;

        public AccountController(HttpClient client)
        {
            _apiUrl = Env.GetString("API_URL");
            _client = client;
        }

        public async Task<Account> Create(string name)
        {
            //Convert account object to json string
            string payload = $"{{name: \"{name}\"}}";

            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_apiUrl + "accounts", content);
            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to Account object
            return JsonSerializer.Deserialize<Account>(responseString);
        }

        public async Task<Account[]> ReadAll()
        {
            //Send a get request for all accounts of the customer hub
            var response = await _client.GetAsync(_apiUrl + "accounts");

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to object with an array of Accounts
            MultiResponse parsedResponse = JsonSerializer.Deserialize<MultiResponse>(responseString);

            return parsedResponse.value;
        }

        public async Task<Account> ReadOne(string id)
        {
            //Send a get request for account matching id
            var response = await _client.GetAsync(_apiUrl + $"accounts({id})");

            string responseString = await response.Content.ReadAsStringAsync();

            //Parse the string from json to Account object
            Account account = JsonSerializer.Deserialize<Account>(responseString);

            return account;
        }

        public async Task Update(string id, Account account)
        {
            //Convert account object to json string
            string payload = $"{{name: \"{account.Name}\"}}";

            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            await _client.PatchAsync(_apiUrl + $"accounts({id})", content);
        }

        public async Task Delete(string id)
        {
            var response = await _client.DeleteAsync(_apiUrl + $"accounts({id})");
        }
    }
}
