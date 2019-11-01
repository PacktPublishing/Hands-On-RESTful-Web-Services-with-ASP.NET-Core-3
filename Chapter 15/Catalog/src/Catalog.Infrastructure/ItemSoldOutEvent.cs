using NServiceBus;

namespace Catalog.Infrastructure
{
    public class ItemSoldOutEvent : IEvent
    {
        public string Id { get; set; }
    }
}