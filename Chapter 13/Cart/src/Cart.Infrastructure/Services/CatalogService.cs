using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Cart.Domain.Responses.Cart;
using Cart.Domain.Services;
using Catalog.API.Client;
using Catalog.API.Contract.Item;

namespace Cart.Infrastructure.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogClient _catalogClient;

        public CatalogService(ICatalogClient catalogClient)
        {
            _catalogClient = catalogClient;
        }

        public async Task<CartItemResponse> EnrichCartItem(
            CartItemResponse item,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _catalogClient.Item.Get(new Guid(item.CartItemId), cancellationToken);
                return Map(item, result);
            }
            catch (Exception)
            {
                return item;
            }
        }

        private static CartItemResponse Map(CartItemResponse item, ItemResponse result)
        {
            item.Description = result.Description;
            item.LabelName = result.LabelName;
            item.Name = result.Name;
            item.Price = result.Price.Amount.ToString(CultureInfo.InvariantCulture);
            item.ArtistName = result.Artist.ArtistName;
            item.GenreDescription = result.Genre.GenreDescription;
            item.PictureUri = result.PictureUri;

            return item;
        }
    }
}