using NServiceBus;

namespace VinylStore.Cart.Events
{
    public class ItemSoldOutEvent : IEvent
    {
        public string Id { get; set; }
    }
}
