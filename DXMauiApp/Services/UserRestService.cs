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
            Uri postUri = new Uri(string.Format(baseUrl + "UserCreate"));
            Uri putUri = new Uri(string.Format(baseUrl + "UserUpdate"));

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
                        email = user.Email,
                        new_email = user.Email,
                        new_password = user.Password,
                        new_image_data = user.Image
                    };
                    json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                }

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                if (isNewItem)
                    response = await _client.PostAsync(postUri, content);
                else
                    response = await _client.PostAsync(putUri, content);

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

        public async Task<HttpResponseMessage> UserConfirmAccessAsync(TokenRequest request)
        {
            Uri uri = new Uri(string.Format(baseUrl + "UserAccessTest"));

            try
            {
                Debug.WriteLine("MODEL IS " + request.Token);

                string json = JsonSerializer.Serialize<TokenRequest>(request, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

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

        public async Task<User> GetUserByTokenAsync(TokenRequest request)
        {
            Uri uri = new Uri(string.Format(baseUrl + "UserGetInfo"));
            User user = new User();

            try
            {
                Debug.WriteLine("MODEL IS " + request.Token);

                string json = JsonSerializer.Serialize<TokenRequest>(request, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    user = JsonSerializer.Deserialize<User>(responseContent, serializerOptions);



                    Debug.WriteLine("RESULT IS: " + user.Email);
                    return user;
                }

                Debug.WriteLine("SERVICE RESPONSE " + response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }

            return null;
        }
    }
}
