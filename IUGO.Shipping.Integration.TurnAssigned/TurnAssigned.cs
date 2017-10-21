using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;
using IUGO.EventBus.AzureServiceFabric.ServiceListener;
using IUGO.Shipping.Integration.TurnAssigned.Handlers;
using IUGO.Turns.Services.Interface.Integration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Shipping.Integration.TurnAssigned
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TurnAssigned : StatelessService
    {
        public TurnAssigned(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            var eventBus = ServiceConfiguration.ConfigureEventBus(
                "Endpoint=sb://fjaramillo.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=mVb/KgmcNz6VMhUf8u+UxXfA3RHusg/eafWcS5KYS18=;EntityPath=turn-assigned"
                , "shipping-turn-assigned");


            return new[]
            {
                new ServiceInstanceListener(context => new ServiceBusEventBusListener<TurnAssignedMessageIntegrationEvent, AssignTurnToShippingHandler>(eventBus), "StatelessService-ServiceBusQueueListener")
            };

        }


    }

    internal class ServiceConfiguration
    {
        public static IEventBus ConfigureEventBus(string serviceBusConnectionString, string subscriptionClientName)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<AssignTurnToShippingHandler>();
            var provider = services.BuildServiceProvider();

            var serviceResolver = new IntegrationHandlersProvider(provider);
            var defaultPersister = new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
            return new EventBusServiceBus(defaultPersister, subscriptionClientName, serviceResolver);
        }
    }
}
