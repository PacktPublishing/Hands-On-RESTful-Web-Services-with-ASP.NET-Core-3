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
            return new Item
            {
                Name = request.Name,
                Description = request.Description,
                LabelName = request.LabelName,
                Price = new Price { Amount = request.Price.Amount, Currency = request.Price.Currency },
                PictureUri = request.PictureUri,
                ReleaseDate = request.ReleaseDate,
                Format = request.Format,
                AvailableStock = request.AvailableStock,
                GenreId = request.GenreId,
                ArtistId = request.ArtistId,
            };
        }

        public Item Map(EditItemRequest request)
        {
            return new Item
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                LabelName = request.LabelName,
                Price = new Price { Amount = request.Price.Amount, Currency = request.Price.Currency },
                PictureUri = request.PictureUri,
                ReleaseDate = request.ReleaseDate,
                Format = request.Format,
                AvailableStock = request.AvailableStock,
                GenreId = request.GenreId,
                ArtistId = request.ArtistId,
            };
        }

        public ItemResponse Map(Item request)
        {
            return new ItemResponse
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                LabelName = request.LabelName,
                Price = new PriceResponse { Amount = request.Price.Amount, Currency = request.Price.Currency },
                PictureUri = request.PictureUri,
                ReleaseDate = request.ReleaseDate,
                Format = request.Format,
                AvailableStock = request.AvailableStock,
                GenreId = request.GenreId,
                Genre = _genreMapper.Map(request.Genre),
                ArtistId = request.ArtistId,
                Artist = _artistMapper.Map(request.Artist),

            };
        }
    }
}