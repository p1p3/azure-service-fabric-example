using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUGO.Turns.Services.Interface;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace IUGO.Turns.Integration.Handlers
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("test")] string message, TextWriter log)
        {
            log.WriteLine(message);
            var turnService = CreateTurnService();
        }


        public static ITurnService CreateTurnService()
        {
            var uri = "fabric:/IUGOsf/IUGO.Turns.Services";

            return ServiceProxy.Create<ITurnService>(
                new Uri(uri),
                new ServicePartitionKey(0));
        }
    }
}
