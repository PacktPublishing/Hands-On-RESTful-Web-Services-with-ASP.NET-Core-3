using AutoMapper;
using Catalog.Domain.Commands.Item;
using Catalog.Domain.Entities;
using Catalog.Domain.Responses.Item;

namespace Catalog.Domain.Infrastructure.Mapper
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