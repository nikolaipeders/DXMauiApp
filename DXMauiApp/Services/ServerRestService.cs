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
    public class ServerRestService : IServerRestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        string baseUrl = "http://51.75.69.121:3000/api/v1/";

        public ServerRestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<HttpResponseMessage> GetStatusAsync()
        {
            Uri uri = new Uri(string.Format(baseUrl + "isOnline"));

            try
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);

                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5)); // Set your desired timeout duration
                var responseTask = _client.SendAsync(httpRequest);

                var completedTask = await Task.WhenAny(responseTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    // Handle the timeout case
                    Debug.WriteLine("GetStatusAsync timed out");
                    throw new TimeoutException("GetStatusAsync timed out");
                }

                // The response task completed within the timeout period
                HttpResponseMessage response = await responseTask;
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                throw; // Rethrow the exception to be handled at a higher level
            }
        }

    }
}
