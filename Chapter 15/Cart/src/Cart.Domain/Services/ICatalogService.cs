using System.Threading;
using System.Threading.Tasks;
using Cart.Domain.Responses.Cart;

namespace Cart.Domain.Services
{
    public interface ICatalogService
    {
        Task<CartItemResponse> EnrichCartItem(
            CartItemResponse item,
            CancellationToken cancellationToken);
    }
}