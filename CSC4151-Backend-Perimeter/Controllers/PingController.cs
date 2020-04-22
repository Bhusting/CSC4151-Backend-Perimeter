using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC4151_Backend_Perimeter.Controllers
{
    [ApiController]    
    [Route("Ping")]
    [Authorize]
    public class PingController : ControllerBase
    {
        public PingController()
        {

        }

        /// <summary>
        /// Returns Pong.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Ping()
        {
            var req = Request;
            return "Pong";
        }
    }
}
