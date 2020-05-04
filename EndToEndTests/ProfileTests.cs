using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;
using Xunit;

namespace EndToEndTests
{
    public class ProfileTests
    {
        [Fact]
        public async Task ProfileTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = new Uri("http://takappservices.azurewebsites.net") };
            //var httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44353") };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await httpClient.GetAsync($"Profile/{Guid.Empty}");

            Assert.True(res.IsSuccessStatusCode);

            var profile = JsonConvert.DeserializeObject<Profile>(await res.Content.ReadAsStringAsync());

            Assert.True(profile.LastName == "Ruxin");
        }
    }
}
