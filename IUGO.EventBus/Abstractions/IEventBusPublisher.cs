using System;

namespace IUGO.EventBus.Abstractions
{
    public interface IEventBusPublisher : IDisposable
    {
        void Publish(IntegrationEvent @event);
    }
}
