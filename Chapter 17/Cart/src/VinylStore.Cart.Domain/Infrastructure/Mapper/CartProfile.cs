using AutoMapper;
using VinylStore.Cart.Domain.Entities;
using VinylStore.Cart.Domain.Responses.Cart;

namespace VinylStore.Cart.Domain.Infrastructure.Mapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItemResponse, CartItem>().ReverseMap();
            CreateMap<CartExtendedResponse, Entities.Cart>().ReverseMap();
            CreateMap<CartUserResponse, CartUser>().ReverseMap();
        }
    }
}
