using System;
using System.Collections.Generic;
using System.Fabric;
using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;
using IUGO.EventBus.AzureServiceFabric.ServiceListener;
using IUGO.Shippings.Integration.TurnAssigned.Handlers;
using IUGO.Shippings.Services.Interfaces;
using IUGO.Turns.Services.Interface.Integration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Shippings.Integration.TurnAssigned
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
            var serviceProvider = ServiceConfiguration.ConfigureEventBus(
                "Endpoint=sb://fjaramillo.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=mVb/KgmcNz6VMhUf8u+UxXfA3RHusg/eafWcS5KYS18=;EntityPath=turn-assigned"
                , "shipping-turn-assigned");


            return new[]
            {
                new ServiceInstanceListener(context => serviceProvider.GetService<ServiceBusEventBusListener<TurnAssignedMessageIntegrationEvent, AssignTurnToShippingHandler>>(), "StatelessService-ServiceBusQueueListener")
            };
        }


    }

    internal class ServiceConfiguration
    {
        public static IServiceProvider ConfigureEventBus(string serviceBusConnectionString, string subscriptionClientName)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<AssignTurnToShippingHandler>();

            services.AddTransient<IShippingService>(context =>
            {
                var uri = "fabric:/IUGOsf/IUGO.Shippings.Services";

                return ServiceProxy.Create<IShippingService>(
                    new Uri(uri),
                    new ServicePartitionKey(0));
            });

            var eventBus = ConfigureEventBus(serviceBusConnectionString, subscriptionClientName, services);

            services.AddSingleton<IEventBusSuscriber>(context => eventBus);
            services.AddTransient<ServiceBusEventBusListener<TurnAssignedMessageIntegrationEvent, AssignTurnToShippingHandler>>();

            var provider = services.BuildServiceProvider();

            return provider;
        }

        public static IEventBusSuscriber ConfigureEventBus(string serviceBusConnectionString, string subscriptionClientName, IServiceCollection serviceCollection)
        {
            var provider = serviceCollection.BuildServiceProvider();
            var serviceResolver = new IntegrationHandlersProvider(provider);
            var defaultPersister = new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
            return new EventBusServiceBusSuscriber(defaultPersister, subscriptionClientName, serviceResolver);
        }
    }
}
