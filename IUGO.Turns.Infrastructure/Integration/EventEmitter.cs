using IUGO.EventBus;
using IUGO.EventBus.Abstractions;

namespace IUGO.Turns.Infrastructure.Integration
{
    public class EventEmitter<T> where T : IntegrationEvent
    {
        private readonly IEventBusPublisher _eventBusPublisher;

        public EventEmitter(IEventBusPublisher eventBusPublisher)
        {
            _eventBusPublisher = eventBusPublisher;
        }

        public void Emit(T eventMessage)
        {
            _eventBusPublisher.Publish(eventMessage);
        }
    }
}
