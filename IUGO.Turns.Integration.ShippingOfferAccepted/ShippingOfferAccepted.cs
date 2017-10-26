using System;
using System.Collections.Generic;
using System.Fabric;
using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;
using IUGO.EventBus.AzureServiceFabric.ServiceListener;
using IUGO.Shippings.Services.Interfaces.Integration;
using IUGO.Turns.Integration.ShippingOfferAccepted.Handlers;
using IUGO.Turns.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Turns.Integration.ShippingOfferAccepted
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class ShippingOfferAccepted : StatelessService
    {
        public ShippingOfferAccepted(StatelessServiceContext context)
            : base(context)
        { }


        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            var serviceProvider = ServiceConfiguration.ConfigureContainer(
                "Endpoint=sb://fjaramillo.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=FvKlnrh/74PqqUZ+8R6EvNa4STsfW+dmvZ/NViWw7NM=;EntityPath=shipping-offer-accepted"
                , "turns-integration");

            return new[]
            {
                new ServiceInstanceListener(context =>serviceProvider.GetService<ServiceBusEventBusListener<ShippingOfferAcceptedIntegrationEvent, AssignShippingToTurn>>(), "StatelessService-ServiceBusQueueListener")
            };
        }
    }
    internal class ServiceConfiguration
    {
        public static IServiceProvider ConfigureContainer(string serviceBusConnectionString, string subscriptionClientName)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<AssignShippingToTurn>();
            services.AddTransient<ITurnService>(context =>
            {
                var uri = "fabric:/IUGOsf/IUGO.Turns.Services";

                return ServiceProxy.Create<ITurnService>(
                    new Uri(uri),
                    new ServicePartitionKey(0));
            });
            
            var eventBus = ConfigureEventBus(serviceBusConnectionString, subscriptionClientName, services);

            services.AddSingleton<IEventBusSuscriber>(context => eventBus);
            services.AddTransient<ServiceBusEventBusListener<ShippingOfferAcceptedIntegrationEvent, AssignShippingToTurn>>();
            
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
