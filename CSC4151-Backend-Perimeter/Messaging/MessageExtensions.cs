using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace CSC4151_Backend_Perimeter.Messaging
{
    public static class MessageExtensions
    {

        public static Message CreateMessage<T>(this Message message, string label, T obj)
        {
            message.Label = label;
            var body = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(obj));
            message.Body = body;
            return message;
        }

    }
}
