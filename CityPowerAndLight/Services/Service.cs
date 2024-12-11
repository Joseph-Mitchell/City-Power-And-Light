using CityPowerAndLight.Models;
using DotNetEnv;
using System.Text.Json;

namespace CityPowerAndLight.Services
{
    internal class Service<TModel> where TModel : Model
    {
        /**Used to deserialize responses with multiple elements
         * in which value is an array containing all elements.
         */
        private class MultiResponse<T>
        {
            public T[] value { get; set; }
        }

        private string _apiUrl;
        private string _apiQuery;
        private HttpClient _client;

        public Service(HttpClient client, string urlSuffix, string query)
        {
            _apiUrl = Env.GetString("API_URL") + urlSuffix;
            _apiQuery = query;
            _client = client;
        }

        /**
         * Takes an http response and converts its content into a string, before parsing
         * the resulting json into an object of the specified type
         */
        private async Task<T> ConvertResponse<T>(HttpResponseMessage message)
        {
            string messageString = await message.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(messageString);
        }

        public async Task<TModel> Create(StringContent content)
        {
            var response = await _client.PostAsync(_apiUrl + _apiQuery, content);

            if (!response.IsSuccessStatusCode) 
                throw new Exception(response.StatusCode.ToString());

            //Convert http response into an object 
            return await ConvertResponse<TModel>(response);
        }

        public async Task<TModel[]> ReadAll()
        {
            var response = await _client.GetAsync(_apiUrl + _apiQuery);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString());

            //Convert http response into an array of objects
            return (await ConvertResponse<MultiResponse<TModel>>(response)).value;
        }

        public async Task<TModel> ReadOne(string id)
        {
            var response = await _client.GetAsync(_apiUrl + $"({id})" + _apiQuery);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString());

            //Convert http response into an object
            return await ConvertResponse<TModel>(response);
        }

        public async Task<TModel> Update(string id, StringContent content)
        {
            var response = await _client.PatchAsync(_apiUrl + $"({id})" + _apiQuery, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString());

            //Convert http response into an object
            return await ConvertResponse<TModel>(response);
        }

        public async Task Delete(string id)
        {
            var response = await _client.DeleteAsync(_apiUrl + $"({id})");

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString());
        }
    }
}
