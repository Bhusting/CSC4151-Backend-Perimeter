using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EndToEndTests
{
    public class TaskTests
    {
        //private readonly Uri _path = new Uri("https://takkapp.azurewebsites.net");
        private readonly Uri _path = new Uri("http://localhost:54381");

        [Fact]
        public async Task GetTaskTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = _path };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var x = Guid.Empty.ToString();

            var res = await httpClient.GetAsync($"Task/House/{x}");

            Assert.True(res.IsSuccessStatusCode);

            var task = JsonConvert.DeserializeObject<List<Domain.Task>>(await res.Content.ReadAsStringAsync());

            Assert.True(task[0].TaskName == "Test");
        }

        [Fact]
        public async Task CreateTaskTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            var httpClient = new HttpClient() { BaseAddress = _path };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var task = new Domain.TaskDTO();
            task.TaskId = Guid.Empty;
            task.HouseId = Guid.Empty;
            task.Channel = Guid.Empty;
            task.TaskName = "EndToEndTests";
            task.Duration = "00:05";
            
            var stringContent = new StringContent(JsonConvert.SerializeObject(task), UnicodeEncoding.UTF8, "application/json");
            var res = await httpClient.PostAsync($"Task", stringContent);
            
            Assert.True(res.IsSuccessStatusCode);

        }
    }
}
