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

        public Task<List<Invite>> GetAllInvitesAsync(TokenRequest token)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> RespondToinviteAsync(TokenRequest request, string response)
        {
            throw new NotImplementedException();
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
