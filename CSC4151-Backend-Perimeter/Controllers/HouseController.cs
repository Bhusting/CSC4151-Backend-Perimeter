using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CSC4151_Backend_Perimeter.Messaging;
using CSC4151_Backend_Perimeter.Queues;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSC4151_Backend_Perimeter.Controllers
{
    [ApiController]
    [Route("House")]
    public class HouseController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IProfileQueueClient _queueClient;

        public HouseController(ILogger<ProfileController> logger, IHttpClientFactory httpClient, IProfileQueueClient queueClient)
        {
            _logger = logger;
            _httpClient = httpClient.CreateClient();
            _queueClient = queueClient;
        }


        [HttpGet("{id}")]
        public async Task<House> GetHouse(Guid id)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/House/{id.ToString()}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var house = JsonConvert.DeserializeObject<House>(await res.Content.ReadAsStringAsync());

            return house;
        }

        [HttpGet("Profile/{id}")]
        public async Task<House> GetHouseByProfile(Guid id)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/House/Profile/{id.ToString()}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var house = JsonConvert.DeserializeObject<House>(await res.Content.ReadAsStringAsync());

            return house;
        }

        [HttpPost("{houseName}")]
        public async Task<ActionResult<Guid>> CreateHouse(string houseName)
        {
            var house = new House();
            house.HouseId = Guid.NewGuid();
            house.HouseName = houseName;
            house.Channel = Guid.NewGuid();
            
            var command = new Message().CreateMessage("CreateHouse", house);

            await _queueClient.Instance.SendAsync(command);
            return Accepted(house.HouseId);
        }

        [HttpDelete("{id}")]
        public async Task DeleteHouse(Guid id)
        {
            var command = new Message().CreateMessage("DeleteHouse", id);

            await _queueClient.Instance.SendAsync(command);
        }
    }
}
