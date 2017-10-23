using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Shippings.Infrastructure.Integration;
using IUGO.Shippings.Services.Interfaces.Integration;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Shippings.Services
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.
                var eventBus = ServiceBusFactory.CreateAzureEventBusInstance(
                    "Endpoint=sb://fjaramillo.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=u/M4nP3NUhfiBF7Ciuk+as6IuqmmBeGyh+l+t2V9orY=;EntityPath=shipping-published",
                    "shipping-services");

                var turnAssignedEventEmitter = new EventEmitter<ShippingPublishedIntegrationEvent>(eventBus);

                var eventBusForOffers = ServiceBusFactory.CreateAzureEventBusInstance(
                    "Endpoint=sb://fjaramillo.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=FvKlnrh/74PqqUZ+8R6EvNa4STsfW+dmvZ/NViWw7NM=;EntityPath=shipping-offer-accepted",
                    "shipping-services");

                var shippingOfferAcceptedEventEmitter = new EventEmitter<ShippingOfferAcceptedIntegrationEvent>(eventBusForOffers);

                ServiceRuntime.RegisterServiceAsync("IUGO.Shippings.ServicesType",
                    context => new ShippingServices(context, turnAssignedEventEmitter)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ShippingServices).Name);

                // Prevents this host process from terminating so services keep running.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
