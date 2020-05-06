using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace CSC4151_Backend_Perimeter.Queues
{
    public interface IChoreQueueClient
    {
        IQueueClient Instance { get; }
    }
}
