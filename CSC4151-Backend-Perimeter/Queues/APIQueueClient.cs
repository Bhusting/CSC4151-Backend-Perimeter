using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace CSC4151_Backend_Perimeter.Queues
{
    public class APIiQueueClient : IProfileQueueClient, ITaskQueueClient, IChoreQueueClient
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        public IQueueClient Instance { get { return Queue.Value;} }
        private Lazy<IQueueClient> Queue => new Lazy<IQueueClient>(() => new QueueClient(_connectionString, _queueName));

        public APIiQueueClient(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }
    }
}
