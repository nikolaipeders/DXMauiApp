using DXMauiApp.Models;
using DXMauiApp.ViewModels;
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

        public async Task<HttpResponseMessage> SaveUserAsync(User user, bool isNewItem = false)
        {
            Uri uri = new Uri(string.Format(baseUrl + "user"));

            try
            {
                string json;
                if (isNewItem)
                {
                    json = JsonSerializer.Serialize(user, _serializerOptions);
                }
                else
                {
                    var jsonObject = new
                    {
                        name = user.name,
                        email = user.email,
                        new_email = user.email,
                        new_password = user.password,
                        new_image_data = user.image
                    };
                    json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                }

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
                Debug.WriteLine($"{response.StatusCode}");
                Debug.WriteLine("REPONSE IS ALSO: " + response);
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
                    Debug.WriteLine("User successfully logged in. Token: " + loginResponse.token);
                    Debug.WriteLine("User ID: " + loginResponse._id);
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
                Debug.WriteLine("MODEL IS " + request.Token);

                // Create a GET request
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);

                // Add a header to the request
                httpRequest.Headers.Add("token", request.Token);

                HttpResponseMessage response = await _client.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("User has access.");
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

        public async Task<User> GetUserByIdAsync(TokenRequest tokenObj, string id)
        {
            var token = tokenObj.Token;
            try
            {
                string apiUrl = $"{baseUrl}user/{id}";

                Debug.WriteLine($" API URL IS {apiUrl}");

                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("token", token);

                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    User user = JsonSerializer.Deserialize<User>(responseContent, serializerOptions);

                    Debug.WriteLine("RESULT IS: " + user.email);
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

        public async Task<HttpResponseMessage> VerifyUserByFaceAsync(User user)
        {
            Uri uri = new Uri(string.Format(baseUrl + "UserVerify"));

            try
            {
                var jsonObject = new
                {
                    email = user.email,
                    image_data = user.image
                };

                string json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                Debug.WriteLine("RESPONSE IS " + response);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("User successfully logged in.");
                }
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
