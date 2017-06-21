using Core.ApplicationServices.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Core.ApplicationServices
{
    public class CustomHttpClient
    {
        private readonly string backendUrl;
        private HttpClient _httpClient;

        public CustomHttpClient()
        {
            backendUrl = ConfigurationManager.AppSettings["backendURL"];
            _httpClient = new HttpClient();
            InitializeClient();
        }

        private void InitializeClient()
        {
            _httpClient.BaseAddress = new Uri(backendUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task PostAuditLog(Dictionary<string, string> data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync("api/logging/audit", content);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
