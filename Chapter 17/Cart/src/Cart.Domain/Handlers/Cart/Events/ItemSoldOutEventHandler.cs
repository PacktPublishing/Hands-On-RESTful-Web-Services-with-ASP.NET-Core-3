using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cart.Domain.Events;
using Cart.Domain.Repositories;
using MediatR;

namespace Cart.Domain.Handlers.Cart.Events
{
    public class ItemSoldOutEventHandler : IRequestHandler<ItemSoldOutEvent>
    {
        private readonly ICartRepository _cartRepository;

        public ItemSoldOutEventHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Unit> Handle(ItemSoldOutEvent @event, CancellationToken cancellationToken)
        {
            var cartIds = _cartRepository.GetCarts().ToList();

            var tasks = cartIds.Select(async x =>
            {
                var cart = await _cartRepository.GetAsync(new Guid(x));
                await RemoveItemsInCart(@event.Id, cart);
            });

            await Task.WhenAll(tasks);

            return Unit.Value;
        }

        private async Task RemoveItemsInCart(string itemToRemove, Entities.Cart cartSessionSession)
        {
            if (string.IsNullOrEmpty(itemToRemove)) return;

            var toDelete = cartSessionSession?.Items?.Where(x => x.CartItemId.ToString() == itemToRemove).ToList();

            if (toDelete == null || toDelete.Count == 0) return;

            foreach (var item in toDelete) cartSessionSession.Items?.Remove(item);

            await _cartRepository.AddOrUpdateAsync(cartSessionSession);
        }
    }
}