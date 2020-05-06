using System;
using System.Collections.Generic;
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
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IProfileQueueClient _queueClient;

        public ProfileController(ILogger<ProfileController> logger, IHttpClientFactory httpClient, IProfileQueueClient queueClient)
        {
            _logger = logger;
            _httpClient = httpClient.CreateClient();
            _queueClient = queueClient;
        }

        [HttpGet("{id}")]
        public async Task<Profile> GetProfile(Guid id)
        {
            _httpClient.BaseAddress = new Uri($"http://profileservice.azurewebsites.net/Profile/{id.ToString()}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var profile = JsonConvert.DeserializeObject<Profile>(await res.Content.ReadAsStringAsync());

            return profile;
        }

        [HttpPost("{firstName}/{lastName}")]
        public async Task<ActionResult<Guid>> CreateProfile(string firstName, string lastName)
        {
            var profile = new Profile();
            profile.ProfileId = Guid.NewGuid();
            profile.FirstName = firstName;
            profile.LastName = lastName;
            profile.XP = 0;
            profile.Email = "Test";

            var command = new Message().CreateMessage("CreateProfile", profile);

            await _queueClient.Instance.SendAsync(command);
            return Accepted(profile.ProfileId);
        }
    }
}
