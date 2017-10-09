using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUGO.Turns.Core.Events;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using ServiceFabric.ServiceBus.Clients;

namespace IUGO.Turns.Infrastructure.Integration
{
    public class TurnAssignedEvent : ITurnAssignedEvent
    {
        private readonly object _payload;
        private readonly string _version;
        private const string Queuename = "turn-assigned";
        private const string Uri = "fabric:/IUGOsf/IUGO.Shipping.Integration.TurnAssigned";

        public TurnAssignedEvent(object payload, string version)
        {
            _payload = payload;
            _version = version;
        }

        public Task Notify()
        {
            //var versionedQueueName = $"{Queuename}.{_version}";
            var versionedQueueName = "test";
            return SendTestMessageToQueue(new Uri(Uri), versionedQueueName, false, _payload);
        }

        private Task SendTestMessageToTopic(Uri uri, string topicName, bool serviceSupportsPartitions, object payload)
        {
            //the name of your application and the name of the Service, the default partition resolver and the topic name
            //to create a communication client factory:
            var resolver = ServicePartitionResolver.GetDefault();
            var factory = new ServiceBusTopicCommunicationClientFactory(resolver, null);

            ServicePartitionClient<ServiceBusTopicCommunicationClient> servicePartitionClient;

            if (serviceSupportsPartitions)
            {
                //determine the partition and create a communication proxy
                var partitionKey = new ServicePartitionKey(0L);
                servicePartitionClient = new ServicePartitionClient<ServiceBusTopicCommunicationClient>(factory, uri, partitionKey);
            }
            else
            {
                servicePartitionClient = new ServicePartitionClient<ServiceBusTopicCommunicationClient>(factory, uri);
            }

            //use the proxy to send a message to the Service
            return servicePartitionClient.InvokeWithRetryAsync(c => c.SendMessageAsync(CreateMessage(payload)));
        }

        private Task SendTestMessageToQueue(Uri uri, string queueName, bool serviceSupportsPartitions, object payload)
        {
            //the name of your application and the name of the Service, the default partition resolver and the topic name
            //to create a communication client factory:
            var factory = new ServiceBusQueueCommunicationClientFactory(ServicePartitionResolver.GetDefault(), queueName);

            ServicePartitionClient<ServiceBusQueueCommunicationClient> servicePartitionClient;

            if (serviceSupportsPartitions)
            {
                //determine the partition and create a communication proxy
                var partitionKey = new ServicePartitionKey(0L);
                servicePartitionClient = new ServicePartitionClient<ServiceBusQueueCommunicationClient>(factory, uri, partitionKey);
            }
            else
            {
                servicePartitionClient = new ServicePartitionClient<ServiceBusQueueCommunicationClient>(factory, uri);
            }

            //use the proxy to send a message to the Service
            return servicePartitionClient.InvokeWithRetryAsync(c => c.SendMessageAsync(CreateMessage(payload)));
        }

        private BrokeredMessage CreateMessage(object payload)
        {
            var message = new BrokeredMessage(payload)
            {

            };

            return message;
        }
    }
}
