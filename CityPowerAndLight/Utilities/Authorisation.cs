using System.Text.Json;

namespace CityPowerAndLight.Utilities
{
    internal class Authorisation
    {
        private struct AuthResponse
        {
            public string access_token { get; set; }
        }

        public async static Task<string> Authorise(HttpClient httpClient)
        {
            var payload = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", DotNetEnv.Env.GetString("CLIENT_ID") },
                { "client_secret", DotNetEnv.Env.GetString("SECRET") },
                { "username", DotNetEnv.Env.GetString("AUTH_USER") },
                { "password", DotNetEnv.Env.GetString("AUTH_PASS") },
                { "scope", DotNetEnv.Env.GetString("AUTH_SCOPE")}
            };

            var response = await httpClient.PostAsync(DotNetEnv.Env.GetString("AUTH_URL"), new FormUrlEncodedContent(payload));
            string responseString = await response.Content.ReadAsStringAsync();
            AuthResponse res = JsonSerializer.Deserialize<AuthResponse>(responseString);
            return JsonSerializer.Deserialize<AuthResponse>(responseString).access_token;
        }
    }
}
