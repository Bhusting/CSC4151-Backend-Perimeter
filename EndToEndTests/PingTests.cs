using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Auth0.OidcClient;
using Domain;
using Newtonsoft.Json;
using Xunit;

namespace EndToEndTests
{
    public class PingTests
    {

        [Fact]
        public async Task PingTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            //var httpClient = new HttpClient() { BaseAddress = new Uri("http://takappservices.azurewebsites.net") };
            var httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await httpClient.GetAsync($"Ping");
            
            Assert.True(res.IsSuccessStatusCode);

            var body = await res.Content.ReadAsStringAsync();

            Assert.True(body == "Pong");
        }
    }
}
