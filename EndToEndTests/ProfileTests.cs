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
        //private readonly Uri _path = new Uri("https://takkapp.azurewebsites.net");
        private readonly Uri _path = new Uri("http://localhost:54381");

        [Fact]
        public async Task GetProfileTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = _path };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var x = Guid.Empty.ToString();

            var res = await httpClient.GetAsync($"Profile/{x}");

            Assert.True(res.IsSuccessStatusCode);

            var profile = JsonConvert.DeserializeObject<Profile>(await res.Content.ReadAsStringAsync());

            Assert.True(profile.LastName == "Ruxin");
        }

        [Fact]
        public async Task GetProfileEmailTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = _path };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var x = "theruxter18@hotmail";

            var res = await httpClient.GetAsync($"Profile/Email/{x}");

            Assert.True(res.IsSuccessStatusCode);

            var profile = JsonConvert.DeserializeObject<Profile>(await res.Content.ReadAsStringAsync());

            Assert.True(profile.LastName == "Ruxin");
        }

        [Fact]
        public async Task GetHouseProfiles()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = _path };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var x = Guid.Empty;

            var res = await httpClient.GetAsync($"Profile/House/{x}");

            Assert.True(res.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CreateProfileTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = _path };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await httpClient.PostAsync($"Profile/Jenny/Macarthur", new StringContent("jmac@gmail.com"));

            Assert.True(res.StatusCode == HttpStatusCode.Accepted);

            var profileId = await res.Content.ReadAsStringAsync();
            
            var getRes = await httpClient.GetAsync($"Profile/{JsonConvert.DeserializeObject<Guid>(profileId)}");
            
            Assert.True(res.IsSuccessStatusCode);

            var profile = JsonConvert.DeserializeObject<Profile>(await getRes.Content.ReadAsStringAsync());
;        }
    }
}
