using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace EndToEndTests
{
    public class PingTests
    {

        [Fact]
        public async Task PingTest()
        {
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44353") };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await httpClient.GetAsync("Ping");

            Assert.True(res.IsSuccessStatusCode);

            var body = await res.Content.ReadAsStringAsync();

            Assert.True(body == "Pong");
        }
    }
}
