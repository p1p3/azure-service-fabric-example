using System;

namespace IUGO.EventBus
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public DateTime CreationDate { get; }

        public const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";
    }
}
