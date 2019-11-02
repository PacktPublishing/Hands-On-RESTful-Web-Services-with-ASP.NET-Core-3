using NServiceBus;

namespace Cart.Events
{
    public class ItemSoldOutEvent : IEvent
    {
        public string Id { get; set; }
    }
}