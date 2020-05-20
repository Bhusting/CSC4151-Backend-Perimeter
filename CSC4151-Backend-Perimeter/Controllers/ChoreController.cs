using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CSC4151_Backend_Perimeter.Messaging;
using CSC4151_Backend_Perimeter.Queues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSC4151_Backend_Perimeter.Controllers
{
    [ApiController]
    [Route("Chore")]
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
        public async Task<Domain.Chore> GetChore(Guid id)
        {
            _httpClient.BaseAddress = new Uri($"https://takchore.azurewebsites.net/Chore/{id.ToString()}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var chore = JsonConvert.DeserializeObject<Domain.Chore>(await res.Content.ReadAsStringAsync());

            return chore;
        }
                

        /// <summary>
        /// Retrieves a Chore from HouseId
        /// </summary>
        /// <param name="houseId">HouseId of the Chore</param>
        /// <returns>List of Chores</returns>
        [HttpGet("House/{houseId}")]
        public async Task<IEnumerable<Domain.Chore>>GetChoresByHouseId(string houseId)
        {
            _httpClient.BaseAddress = new Uri($"https://takchore.azurewebsites.net/Chore/House/{houseId}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var chores = JsonConvert.DeserializeObject<IEnumerable<Domain.Chore>>(await res.Content.ReadAsStringAsync());

            return chores;
        }

        /// <summary>
        /// Retrieves a Chore from ChoreTypeId
        /// </summary>
        /// <param name="choretypeId">ChoreTypeId of the Chore</param>
        /// <returns>List of Chores</returns>
        [HttpGet("ChoreType/{choreTypeId}")]
        public async Task<IEnumerable<Domain.Chore>> GetChoresByChoreTypeId(short choreTypeId)
        {
            _httpClient.BaseAddress = new Uri($"https://takchore.azurewebsites.net/Chore/ChoreType/{choreTypeId}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var chores = JsonConvert.DeserializeObject<IEnumerable<Domain.Chore>>(await res.Content.ReadAsStringAsync());

            return chores;
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateChore([FromBody] Domain.Chore chore)
        {
            chore.ChoreId = Guid.NewGuid();
            
            var command = new Message().CreateMessage("CreateChore", chore);

            await _queueClient.Instance.SendAsync(command);

            return Accepted(chore.ChoreId);
        }

        [HttpDelete("{id}")]
        public async Task DeleteChore(Guid id)
        {

            var command = new Message().CreateMessage("DeleteChore", id);

            await _queueClient.Instance.SendAsync(command);
        }
    }
}
