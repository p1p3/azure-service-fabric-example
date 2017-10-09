using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Shipping.Integration.TurnAssigned;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.ServiceBus.Services;
using ServiceFabric.ServiceBus.Services.CommunicationListeners;

namespace IUGO.Shipping.Integration.TurnAssigned
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TurnAssigned : StatelessService
    {
        public TurnAssigned(StatelessServiceContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            // In the configuration file, define connection strings: 
            // "Microsoft.ServiceBus.ConnectionString.Receive"
            // and "Microsoft.ServiceBus.ConnectionString.Send"

            // Also, define a QueueName:
            string serviceBusQueueName = "test"; //using entity path.
            //alternative: CloudConfigurationManager.GetSetting("QueueName");
            Action<string> logAction = log => ServiceEventSource.Current.ServiceMessage(base.Context, log);
            yield return new ServiceInstanceListener(context => new ServiceBusQueueCommunicationListener(
                new Handler(logAction)
                , context
                , serviceBusQueueName
                , requireSessions: false)
            {
                AutoRenewTimeout =
                    TimeSpan.FromSeconds(
                        70), //auto renew up until 70s, so processing can take no longer than 60s (default lock duration).
                LogAction = logAction,
                MessagePrefetchCount = 10
            }, "StatelessService-ServiceBusQueueListener");
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }

    internal sealed class Handler : AutoCompleteServiceBusMessageReceiver
    {

        public Handler(Action<string> logAction)
            : base(logAction)
        {
        }


        protected override Task ReceiveMessageImplAsync(BrokeredMessage message, MessageSession session,
            CancellationToken cancellationToken)
        {
            WriteLog(
                $"Sleeping for 7s while processing queue message {message.MessageId} to test message lock renew function (send more than 9 messages!).");
            Thread.Sleep(TimeSpan.FromSeconds(7));

            WriteLog($"Handling queue message {message.MessageId}");
            return Task.FromResult(true);
        }

    }
}
