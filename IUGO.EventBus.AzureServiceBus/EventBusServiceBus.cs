using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using Microsoft.Azure.ServiceBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IUGO.EventBus.AzureServiceBus
{
    public class EventBusServiceBus : IEventBus
    {
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly SubscriptionClient _subscriptionClient;

        //private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "IUGO_event_bus";

        private const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";
        private readonly IHandlerServiceProvider _handlerServiceProvider;


        public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection,
            IEventBusSubscriptionsManager subsManager, string subscriptionClientName, IHandlerServiceProvider handlerServiceProvider)
        {
            _handlerServiceProvider = handlerServiceProvider;
            _serviceBusPersisterConnection = serviceBusPersisterConnection;
            _subsManager = subsManager;

            _subscriptionClient = new SubscriptionClient(serviceBusPersisterConnection.ServiceBusConnectionStringBuilder,
                subscriptionClientName);

            RemoveDefaultRule();
            RegisterSubscriptionClientMessageHandler();
        }

        public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection, string subscriptionClientName) : this(serviceBusPersisterConnection, new InMemoryEventBusSubscriptionsManager(), subscriptionClientName, null)
        {
        }

        public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection, string subscriptionClientName, IHandlerServiceProvider sericeProvider) : this(serviceBusPersisterConnection, new InMemoryEventBusSubscriptionsManager(), subscriptionClientName, sericeProvider)
        {
        }

        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFIX, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = new Guid().ToString(),
                Body = body,
                Label = eventName,
            };

            var topicClient = _serviceBusPersisterConnection.CreateModel();

            topicClient.SendAsync(message)
                .GetAwaiter()
                .GetResult();
        }

        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");

            var containsKey = _subsManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                _subscriptionClient.AddRuleAsync(new RuleDescription
                {
                    Filter = new CorrelationFilter { Label = eventName },
                    Name = eventName
                }).GetAwaiter().GetResult();
            }

            _subsManager.AddSubscription<T, TH>();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");

            _subscriptionClient
             .RemoveRuleAsync(eventName)
             .GetAwaiter()
             .GetResult();

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            _subsManager.Clear();
        }

        private void RegisterSubscriptionClientMessageHandler()
        {
            _subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var eventName = $"{message.Label}{INTEGRATION_EVENT_SUFIX}";
                    var messageData = Encoding.UTF8.GetString(message.Body);
                    await ProcessEvent(eventName, messageData);

                    // Complete the message so that it is not received again.
                    await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                },
               new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var handler = _handlerServiceProvider.CreateInstance(subscription.HandlerType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
        }

        private void RemoveDefaultRule()
        {
            var defaultRuleExist = _subscriptionClient.GetRulesAsync()
                .GetAwaiter().GetResult()
                .Any(rule => rule.Name == RuleDescription.DefaultRuleName);

            if (defaultRuleExist)
            {
                _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
