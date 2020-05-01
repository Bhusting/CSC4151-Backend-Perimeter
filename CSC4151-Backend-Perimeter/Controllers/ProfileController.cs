﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CSC4151_Backend_Perimeter.Controllers
{
    [ApiController]
    [Route("Profile")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public ProfileController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://52.149.194.40/Profile");
        }

        [HttpGet("{id}")]
        public async Task<Profile> GetProfile(Guid id)
        {
            var res = await _httpClient.GetAsync($"{id}");

            var profile = JsonConvert.DeserializeObject<Profile>(await res.Content.ReadAsStringAsync());

            return profile;
        }
    }
}
