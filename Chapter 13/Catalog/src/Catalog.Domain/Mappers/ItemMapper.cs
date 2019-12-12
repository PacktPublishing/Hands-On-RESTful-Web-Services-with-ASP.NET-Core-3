using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Mappers
{
    public class ItemMapper : IItemMapper
    {
        private readonly IArtistMapper _artistMapper;
        private readonly IGenreMapper _genreMapper;

        public ItemMapper(IArtistMapper artistMapper, IGenreMapper genreMapper)
        {
            _artistMapper = artistMapper;
            _genreMapper = genreMapper;
        }

        public Item Map(AddItemRequest request)
        {
            if (request == null) return null;
            
            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                LabelName = request.LabelName,
                PictureUri = request.PictureUri,
                ReleaseDate = request.ReleaseDate,
                Format = request.Format,
                AvailableStock = request.AvailableStock,
                GenreId = request.GenreId,
                ArtistId = request.ArtistId,
            };

            if (request.Price != null)
            {
                item.Price = new Price { Currency = request.Price.Currency, Amount = request.Price.Amount };
            }

            return item;
        }

        public Item Map(EditItemRequest request)
        {
            if (request == null) return null;
            
            var item = new Item
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                LabelName = request.LabelName,
                PictureUri = request.PictureUri,
                ReleaseDate = request.ReleaseDate,
                Format = request.Format,
                AvailableStock = request.AvailableStock,
                GenreId = request.GenreId,
                ArtistId = request.ArtistId,
            };
            
            if (request.Price != null)
            {
                item.Price = new Price { Currency = request.Price.Currency, Amount = request.Price.Amount };
            }

            return item;
        }

        public ItemResponse Map(Item request)
        {
            if (request == null) return null;
            
            var response = new ItemResponse
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                LabelName = request.LabelName,
                PictureUri = request.PictureUri,
                ReleaseDate = request.ReleaseDate,
                Format = request.Format,
                AvailableStock = request.AvailableStock,
                GenreId = request.GenreId,
                Genre = _genreMapper.Map(request.Genre),
                ArtistId = request.ArtistId,
                Artist = _artistMapper.Map(request.Artist),
            };
            
            if (request.Price != null)
            {
                response.Price = new PriceResponse { Currency = request.Price.Currency, Amount = request.Price.Amount };
            }

            return response;
        }
    }
}