using AutoMapper;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Entities;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Infrastructure.Mapper
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<ItemResponse, Item>().ReverseMap();
            CreateMap<GenreResponse, Genre>().ReverseMap();
            CreateMap<ArtistResponse, Artist>().ReverseMap();
            CreateMap<Money, MoneyResponse>().ReverseMap();
            CreateMap<AddItemCommand, Item>().ReverseMap();
            CreateMap<EditItemCommand, Item>().ReverseMap();
        }
    }
}
