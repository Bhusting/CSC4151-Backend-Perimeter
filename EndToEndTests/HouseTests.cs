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
    public class HouseTests
    {
        [Fact]
        public async Task GetHouseTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = new Uri("https://takkapp.azurewebsites.net") };
            //var httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:54381") };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var x = Guid.Empty.ToString();

            var res = await httpClient.GetAsync($"House/{x}");

            Assert.True(res.IsSuccessStatusCode);

            var house = JsonConvert.DeserializeObject<House>(await res.Content.ReadAsStringAsync());

            Assert.True(house.HouseId == Guid.Empty);
        }
    }
}
