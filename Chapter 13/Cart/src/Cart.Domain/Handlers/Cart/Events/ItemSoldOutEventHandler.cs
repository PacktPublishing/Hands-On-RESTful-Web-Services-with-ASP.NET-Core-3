using System;
using System.Linq;
using System.Threading.Tasks;
using Cart.Domain.Infrastructure.Repositories;
using Cart.Events;
using NServiceBus;

namespace Cart.Domain.Handlers.Cart.Events
{
    public class ItemSoldOutEventHandler : IHandleMessages<ItemSoldOutEvent>
    {
        private readonly ICartRepository _cartRepository;

        public ItemSoldOutEventHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Task Handle(ItemSoldOutEvent @event, IMessageHandlerContext context)
        {
            var cartIds = _cartRepository.GetCarts().ToList();

            var tasks = cartIds.Select(async x =>
            {
                var cart = await _cartRepository.GetAsync(new Guid(x));
                await RemoveItemsInCart(@event.Id, cart);
            });

            Task.WhenAll(tasks);
            return Task.CompletedTask;
        }

        private async Task RemoveItemsInCart(string itemToRemove, Entities.Cart cart)
        {
            if (string.IsNullOrEmpty(itemToRemove)) return;

            var toDelete = cart?.Items?.Where(x => x.CartItemId.ToString() == itemToRemove).ToList();

            if (toDelete == null || toDelete.Count == 0) return;

            foreach (var item in toDelete) cart.Items?.Remove(item);

            await _cartRepository.AddOrUpdateAsync(cart);
        }
    }
}