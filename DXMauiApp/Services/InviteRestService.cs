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
    public class InviteRestService : IInviteRestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        string baseUrl = "http://51.75.69.121:3000/api/v1/";

        public InviteRestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<List<Invite>> GetAllInvitesAsync(TokenRequest token, string user_id)
        {
            try
            {
                string apiUrl = baseUrl + $"invite/search/{user_id}";

                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("token", token.Token);

                Debug.WriteLine("TOKEN INVITE GET ALL IS " + token.Token);
                Debug.WriteLine("TOKEN INVITE ID " + user_id);

                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    List<Invite> invites = JsonSerializer.Deserialize<List<Invite>>(responseContent, serializerOptions);

                    return invites;
                }

                // Throw an exception instead of returning null
                throw new Exception("Failed to retrieve invite information. Status code: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> RespondToInviteAsync(TokenRequest request, string id, string answer)
        {
            Debug.WriteLine("RESPOND SERVICE CALLED");
            Uri uri = new Uri(baseUrl + $"invite/respond/{id}/{answer}");

            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
                requestMessage.Headers.Add("token", request.Token);

                HttpResponseMessage response = await _client.SendAsync(requestMessage);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<HttpResponseMessage> SendInviteAsync(TokenRequest request, Lock exiLock, string mail)
        {
            Uri uri = new Uri(baseUrl + "invite/send");

            try
            {
                var jsonObject = new
                {
                    toEmail = mail,
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
