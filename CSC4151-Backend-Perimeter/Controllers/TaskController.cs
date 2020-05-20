using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CSC4151_Backend_Perimeter.Messaging;
using CSC4151_Backend_Perimeter.Queues;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace CSC4151_Backend_Perimeter.Controllers
{
    [ApiController]
    [Route("Task")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly HttpClient _httpClient;
        private readonly ITaskQueueClient _queueClient;

        public TaskController(ILogger<TaskController> logger, IHttpClientFactory httpClient, ITaskQueueClient queueClient)
        {
            _logger = logger;
            _httpClient = httpClient.CreateClient();
            _queueClient = queueClient;
        }

        [HttpGet("House/{houseId}")]
        public async Task<List<Domain.Task>> GetTaskByHouse(Guid houseId)
        {
            _httpClient.BaseAddress = new Uri($"https://taktask.azurewebsites.net/Task/House/{houseId}");
            var res = await _httpClient.GetAsync("");

            _logger.LogInformation(res.StatusCode.ToString());

            var tasks = JsonConvert.DeserializeObject<List<Domain.Task>>(await res.Content.ReadAsStringAsync());

            return tasks;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Domain.TaskDTO taskDTO)
        {
            var task = new TaskDTO();

            task.TaskId = Guid.NewGuid();
            task.TaskName = taskDTO.TaskName;
            task.Channel = taskDTO.Channel;
            task.HouseId = taskDTO.HouseId;
            task.Duration = taskDTO.Duration;


            var command = new Message().CreateMessage("CreateTask", task);

            await _queueClient.Instance.SendAsync(command);
            return Accepted(task.TaskId);
        }

        [HttpDelete("{taskId}")]
        public async Task DeleteTask(Guid taskId)
        {
            var command = new Message().CreateMessage("CreateTask", taskId);

            await _queueClient.Instance.SendAsync(command);
        }
    }
}
