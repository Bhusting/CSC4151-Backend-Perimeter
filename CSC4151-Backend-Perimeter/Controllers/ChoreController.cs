using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CSC4151_Backend_Perimeter.Messaging;
using CSC4151_Backend_Perimeter.Queues;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSC4151_Backend_Perimeter.Controllers
{
    [ApiController]
    [Route("Profile")]
    [Authorize]
    public class ChoreController : ControllerBase
    {
        private readonly ILogger<ChoreController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IChoreQueueClient _queueClient;

        public ChoreController(ILogger<ChoreController> logger, IHttpClientFactory httpClient, IChoreQueueClient queueClient)
        {
            _logger = logger;
            _httpClient = httpClient.CreateClient();
            _queueClient = queueClient;
        }

        [HttpGet("{id}")]
        public async Task<Chore> GetChore(Guid id)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/Chore/{id.ToString()}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var chore = JsonConvert.DeserializeObject<Chore>(await res.Content.ReadAsStringAsync());

            return chore;
        }
                

        /// <summary>
        /// Retrieves a Chore from HouseId
        /// </summary>
        /// <param name="houseId">HouseId of the Chore</param>
        /// <returns>List of Chores</returns>
        [HttpGet("HouseId/{houseId}")]
        public async Task<IEnumerable<Chore>>GetChoresByHouseId(string houseId)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/Chore/HouseId/{houseId}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var chores = JsonConvert.DeserializeObject<IEnumerable<Chore>>(await res.Content.ReadAsStringAsync());

            return chores;
        }

        /// <summary>
        /// Retrieves a Chore from ChoreTypeId
        /// </summary>
        /// <param name="choretypeId">ChoreTypeId of the Chore</param>
        /// <returns>List of Chores</returns>
        [HttpGet("ChoreTypeId/{choretypeId}")]
        public async Task<IEnumerable<Chore>> GetChoresByChoreTypeId(short choretypeId)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/Chore/ChoreTypeId/{choretypeId}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var chores = JsonConvert.DeserializeObject<IEnumerable<Chore>>(await res.Content.ReadAsStringAsync());

            return chores;
        }
        
        [HttpPost]
        public async Task<string> CreateChore([FromBody] Chore chore)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/Chore");
            var json = JsonConvert.SerializeObject(chore);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync($"Chore", stringContent);

            _logger.LogInformation(res.StatusCode.ToString());

            var body = await res.Content.ReadAsStringAsync();
            return body;

        }

        [HttpDelete("{id}")]
        public async Task DeleteChore(Guid id)
        {

            var command = new Message().CreateMessage("DeleteChore", id);

            await _queueClient.Instance.SendAsync(command);
        }
    }
}
