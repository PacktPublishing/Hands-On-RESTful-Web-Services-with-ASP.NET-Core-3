using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cart.Domain.Entities;
using Cart.Domain.Repositories;
using Cart.Domain.Responses.Cart;
using Cart.Domain.Services;
using Moq;
using Newtonsoft.Json;

namespace Cart.Fixtures
{
    public class CartContextFactory
    {
        public ICartRepository GetCartRepository()
        {
            var cartRepository = new Mock<ICartRepository>();
            var memoryCollection = GetMemoryCollection();

            cartRepository
                .Setup(x => x.GetCarts())
                .Returns(() => memoryCollection.Keys.Select(x => x.ToString()).ToList());


            cartRepository
                .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .Returns((Guid id) =>
                    Task.FromResult(
                        JsonConvert.DeserializeObject<CartSession>(memoryCollection[id.ToString()])));

            cartRepository
                .Setup(x => x.AddOrUpdateAsync(It.IsAny<CartSession>()))
                .Callback((CartSession x) =>
                {
                    memoryCollection.AddOrUpdate(x.Id, JsonConvert.SerializeObject(x),
                        (id, value) => JsonConvert.SerializeObject(x));
                })
                .ReturnsAsync((CartSession x) =>
                    JsonConvert.DeserializeObject<CartSession>(memoryCollection[x.Id]));

            return cartRepository.Object;
        }

        public ICatalogService GetCatalogService()
        {
            var catalogService = new Mock<ICatalogService>();
            var itemResponse = GetItemResponse();

            catalogService
                .Setup(x => x.EnrichCartItem(It.IsAny<CartItemResponse>(), CancellationToken.None))
                .ReturnsAsync((CartItemResponse item, CancellationToken token) =>
                {
                    item.Description = itemResponse[new Guid(item.CartItemId)].Description;
                    item.Name = itemResponse[new Guid(item.CartItemId)].Name;
                    item.Price = itemResponse[new Guid(item.CartItemId)].Price;
                    item.ArtistName = itemResponse[new Guid(item.CartItemId)].ArtistName;
                    item.GenreDescription = itemResponse[new Guid(item.CartItemId)].GenreDescription;
                    item.PictureUri = itemResponse[new Guid(item.CartItemId)].PictureUri;
                    return item;
                });

            return catalogService.Object;
        }

        private static ConcurrentDictionary<string, string> GetMemoryCollection()
        {
            using (var reader = new StreamReader("./Data/cart.json"))
            {
                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<CartSession[]>(json);

                var memoryCollection = data.ToDictionary(cart => cart.Id, JsonConvert.SerializeObject);

                var concurrentDictionary = new ConcurrentDictionary<string, string>(memoryCollection);
                return concurrentDictionary;
            }
        }

        private static Dictionary<Guid, CartItemResponse> GetItemResponse()
        {
            using (var reader = new StreamReader("./Data/items.json"))
            {
                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<CartItemResponse[]>(json);

                return data.ToDictionary(item => new Guid(item.CartItemId), item => item);
            }
        }
    }
}