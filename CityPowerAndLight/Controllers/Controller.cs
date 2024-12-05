﻿using CityPowerAndLight.Models;
using DotNetEnv;
using System.Text.Json;

namespace CityPowerAndLight.Controllers
{
    internal class Controller<TModel> where TModel : Model
    {
        /**Used to deserialize responses with multiple elements
         * in which value is an array containing all elements.
         */
        private class MultiResponse<T>
        {
            public T[] value { get; set; }
        }

        private string _apiUrl;
        private HttpClient _client;

        public Controller(HttpClient client, string urlSuffix)
        {
            _apiUrl = Env.GetString("API_URL") + urlSuffix;
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

        public async Task<TModel> Create(TModel model)
        {
            //Convert json payload into compatible content for request
            var content = new StringContent(model.GetPayload(), System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_apiUrl, content);

            //Convert http response into an object 
            return await ConvertResponse<TModel>(response);
        }

        public async Task<TModel[]> ReadAll()
        {
            var response = await _client.GetAsync(_apiUrl);

            //Convert http response into an array of objects
            return (await ConvertResponse<MultiResponse<TModel>>(response)).value;
        }

        public async Task<TModel> ReadOne(string id)
        {
            var response = await _client.GetAsync(_apiUrl + $"({id})");

            //Convert http response into an object
            return await ConvertResponse<TModel>(response);
        }

        public async Task<TModel> Update(string id, TModel model)
        {
            //Convert json payload into compatible content for request
            var content = new StringContent(model.GetPayload(), System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync(_apiUrl + $"({id})", content);

            //Convert http response into an object
            return await ConvertResponse<TModel>(response);
        }

        public async Task Delete(string id)
        {
            var response = await _client.DeleteAsync(_apiUrl + $"({id})");
        }
    }
}
