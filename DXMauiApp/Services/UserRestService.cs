using DXMauiApp.Models;
using DXMauiApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public class UserRestService : IUserRestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        string baseUrl = "http://51.75.69.121:3000/api/v1/";

        public UserRestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<HttpResponseMessage> CreateUserAsync(User user)
        {
            Uri uri = new Uri(string.Format(baseUrl + "user"));

            try
            {
                string json = JsonSerializer.Serialize(user, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> UpdateUserAsync(TokenRequest token, User user)
        {
            Uri uri = new Uri(string.Format(baseUrl + "user"));

            try
            {
                var jsonObject = new
                {
                    email = user.email,
                    name = user.name,
                    password = user.password,
                    image = user.image
                };
                string json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.Add("token", token.Token);

                HttpResponseMessage response = await _client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                return null;
            }
        }

        public async Task<LoginResponse> UserLoginAsync(User user)
        {
            Uri uri = new Uri(string.Format(baseUrl + "login"));

            try
            {
                string json = JsonSerializer.Serialize<User>(user, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    LoginResponse loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseJson);
                    return loginResponse;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                return null;
            }
        }


        public async Task<HttpResponseMessage> UserConfirmAccessAsync(TokenRequest request)
        {
            Uri uri = new Uri(string.Format(baseUrl + "debug/userAccess"));

            try
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);

                httpRequest.Headers.Add("token", request.Token);

                HttpResponseMessage response = await _client.SendAsync(httpRequest);
                
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(TokenRequest request, string id)
        {
            try
            {
                string apiUrl = $"{baseUrl}user/{id}";

                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("token", request.Token);

                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    User user = JsonSerializer.Deserialize<User>(responseContent, serializerOptions);

                    return user;
                }

                // Throw an exception instead of returning null
                throw new Exception("Failed to retrieve user information. Status code: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> VerifyUserByFaceAsync(TokenRequest request, Lock exiLock, string image)
        {
            Uri uri = new Uri(baseUrl + "verifyFace");

            try
            {
                var jsonObject = new
                {
                    image_data = image,
                    lock_id = exiLock._id,
                };

                string json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add a header
                content.Headers.Add("token", request.Token);

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                requestMessage.Content = content;

                HttpResponseMessage response = await _client.SendAsync(requestMessage);

                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("RESPONSE BODY: " + responseBody);

                Debug.WriteLine("RESPONSE FROM FACE IS " + response.StatusCode);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
