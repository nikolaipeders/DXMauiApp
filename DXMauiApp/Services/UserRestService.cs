using DXMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public class UserRestService : IUserRestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        string baseUrl = "http://51.75.69.121:3000/api/";

        public UserRestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<HttpResponseMessage> SaveUserAsync(User user, bool isNewItem = false)
        {
            Uri uri = new Uri(string.Format(baseUrl + "UserCreate"));

            try
            {
                string json = JsonSerializer.Serialize<User>(user, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                if (isNewItem)
                    response = await _client.PostAsync(uri, content);

                else
                    response = await _client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("User successfully saved.");
                    return response;
                }
                return (response);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                return null;
            }
        }

        public async Task<string> UserLoginAsync(User user)
        {
            Uri uri = new Uri(string.Format(baseUrl + "UserLogin"));

            try
            {
                string json = JsonSerializer.Serialize<User>(user, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("User successfully logged in.");
                    string token = await response.Content.ReadAsStringAsync();
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> UserSignOutAsync(TokenRequest request)
        {
            Uri uri = new Uri(string.Format(baseUrl + "UserLogout"));

            try
            {
                Debug.WriteLine("MODEL IS " + request.Token);

                string json = JsonSerializer.Serialize<TokenRequest>(request, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("User successfully signed out.");
                    return response;
                }
                Debug.WriteLine("SERVICE RESPONSE " + response.Content.ReadAsStringAsync().Result);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                return null;
            }
        }
    }
}
