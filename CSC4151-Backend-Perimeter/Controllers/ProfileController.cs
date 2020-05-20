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
        public async Task<Domain.Profile> GetProfile(Guid id)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/Profile/{id.ToString()}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var profile = JsonConvert.DeserializeObject<Domain.Profile>(await res.Content.ReadAsStringAsync());

            return profile;
        }
        
        /// <summary>
        /// Retrieves a Users XP Value.
        /// </summary>
        /// <param name="id">Id of the profile to retrieve.</param>
        /// <returns>XP value.</returns>
        [HttpGet("{profileId}/XP")]
        public async Task<int> GetXP(Guid profileId)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/Profile/{profileId.ToString()}/XP");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var xp = JsonConvert.DeserializeObject<int>(await res.Content.ReadAsStringAsync());

            return xp;
        }

        /// <summary>
        /// Retrieves a User from Email
        /// </summary>
        /// <param name="email">Email of the User</param>
        /// <returns>User Profile</returns>
        [HttpGet("Email/{email}")]
        public async Task<Domain.Profile> GetProfileByEmail(string email)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/Profile/Email/{email}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var profile = JsonConvert.DeserializeObject<Domain.Profile>(await res.Content.ReadAsStringAsync());

            return profile;
        }

        [HttpGet("House/{houseId}")]
        public async Task<List<Domain.Profile>> GetHouseProfiles(Guid houseId)
        {
            _httpClient.BaseAddress = new Uri($"https://takprofile.azurewebsites.net/Profile/House/{houseId}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var profiles = JsonConvert.DeserializeObject<List<Domain.Profile>>(await res.Content.ReadAsStringAsync());

            return profiles;
        }

        [HttpPost("{firstName}/{lastName}")]
        public async Task<ActionResult<Guid>> CreateProfile(string firstName, string lastName)
        {
            var profile = new Domain.Profile();
            profile.ProfileId = Guid.NewGuid();
            profile.FirstName = firstName;
            profile.LastName = lastName;
            profile.XP = 0;

            string email;
            using (StreamReader stream = new StreamReader(Request.Body))
            { 
                email= await stream.ReadToEndAsync();
            }

            profile.Email = email;

            var command = new Message().CreateMessage("CreateProfile", profile);

            await _queueClient.Instance.SendAsync(command);
            return Accepted(profile.ProfileId);
        }

        [HttpPost("{profileId}/House/{houseId}")]
        public async Task UpdateHouse(Guid profileId, Guid houseId)
        {
            var command = new Message().CreateMessage("UpdateHouse", new KeyValuePair<Guid, Guid>(profileId, houseId));

            await _queueClient.Instance.SendAsync(command);
        }

        [HttpPost("{profileId}/XP")]
        public async Task AddXP(Guid profileId)
        {
            var command = new Message().CreateMessage("AddXP", profileId);

            await _queueClient.Instance.SendAsync(command);
        }

        [HttpDelete("{id}")]
        public async Task DeleteProfile(Guid id)
        {
            var command = new Message().CreateMessage("DeleteProfile", id);

            await _queueClient.Instance.SendAsync(command);
        }
    }
}
