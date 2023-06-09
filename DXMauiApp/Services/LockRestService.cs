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
    public class LockRestService : ILockRestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        string baseUrl = "http://51.75.69.121:3000/api/v1/";

        public LockRestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<HttpResponseMessage> SaveLockAsync(TokenRequest request, Lock newLock, bool isNewItem = false)
        {
            Uri uri = new Uri(string.Format(baseUrl + "lock"));

            try
            {
                string json;
                if (!isNewItem)
                {
                    json = JsonSerializer.Serialize(newLock, _serializerOptions);
                }
                else
                {
                    var jsonObject = new
                    {
                        serial = newLock.serial,
                        location = newLock.location,
                        name = newLock.name,
                    };
                    json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                }

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add a header
                content.Headers.Add("token", request.Token);

                HttpResponseMessage response = null;

                if (isNewItem)
                    response = await _client.PostAsync(uri, content);
                else
                    response = await _client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> DeleteLockAsync(TokenRequest request, Lock exiLock)
        {
            Uri uri = new Uri(baseUrl + "lock");

            try
            {
                var jsonObject = new
                {
                    _id = exiLock._id
                };

                string json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add a header
                content.Headers.Add("token", request.Token);

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
                requestMessage.Content = content;

                HttpResponseMessage response = await _client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Lock>> GetAllLocksAsync(TokenRequest token)
        {
            try
            {
                string apiUrl = $"{baseUrl}lock";

                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("token", token.Token);

                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    List<Lock> locks = JsonSerializer.Deserialize<List<Lock>>(responseContent, serializerOptions);

                    return locks;
                }

                // Throw an exception instead of returning null
                throw new Exception("Failed to retrieve lock information. Status code: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        public async Task<Lock> GetLockByIdAsync(TokenRequest token, string id)
        {
            try
            {
                string apiUrl = $"{baseUrl}lock/{id}";

                Debug.WriteLine("URL IS " + apiUrl);
                Debug.WriteLine("TOKEN IS " + token.Token);

                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("token", token.Token);

                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    Lock exiLock = JsonSerializer.Deserialize<Lock>(responseContent, serializerOptions);

                    return exiLock ;
                }

                // Throw an exception instead of returning null
                throw new Exception("Failed to retrieve lock information. Status code: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> RemoveAccessAsync(TokenRequest request, Lock exiLock, User user)
        {
            Uri uri = new Uri(baseUrl + "lock/remove_access");

            try
            {
                var jsonObject = new
                {
                    lock_id = exiLock._id,
                    user_id = user._id
                };

                string json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add a header
                content.Headers.Add("token", request.Token);

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                requestMessage.Content = content;
                requestMessage.Headers.Add("token", request.Token);

                HttpResponseMessage response = await _client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        } 
        
        public async Task<HttpResponseMessage> LeaveLockAsync(TokenRequest request, Lock exiLock)
        {
            Uri uri = new Uri(baseUrl + "lock/leave");

            try
            {
                var jsonObject = new
                {
                    lock_id = exiLock._id
                };

                string json = JsonSerializer.Serialize(jsonObject, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add a header
                content.Headers.Add("token", request.Token);

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                requestMessage.Content = content;
                requestMessage.Headers.Add("token", request.Token);

                HttpResponseMessage response = await _client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
