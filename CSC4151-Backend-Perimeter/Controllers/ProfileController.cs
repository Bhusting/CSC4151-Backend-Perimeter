using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ProfileController(ILogger<ProfileController> logger, IHttpClientFactory httpClient)
        {
            _logger = logger;
            _httpClient = httpClient.CreateClient();
            _httpClient.BaseAddress = new Uri("http://profileservice.azurewebsites.net/Profile");
        }

        [HttpGet("{id}")]
        public async Task<Profile> GetProfile(Guid id)
        {
            var res = await _httpClient.GetAsync($"{id}");

            _logger.LogInformation(res.StatusCode.ToString());

            var profile = JsonConvert.DeserializeObject<Profile>(await res.Content.ReadAsStringAsync());

            return profile;
        }
    }
}
