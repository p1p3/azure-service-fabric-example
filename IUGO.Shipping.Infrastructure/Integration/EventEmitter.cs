using IUGO.EventBus;
using IUGO.EventBus.Abstractions;

namespace IUGO.Shippings.Infrastructure.Integration
{
    public class EventEmitter<T> where T : IntegrationEvent
    {
        private readonly IEventBus _eventBus;

        public EventEmitter(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Emit(T eventMessage)
        {
            _eventBus.Publish(eventMessage);
        }
    }
}
