using MediatR;

namespace Cart.Domain.Events
{
    public class ItemSoldOutEvent : IRequest<Unit>
    {
        public string Id { get; set; }
    }
}