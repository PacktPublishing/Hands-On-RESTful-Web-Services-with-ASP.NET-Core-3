using AutoMapper;
using Cart.Domain.Entities;
using Cart.Domain.Responses.Cart;

namespace Cart.Domain.Infrastructure.Mapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItemResponse, CartItem>().ReverseMap();
            CreateMap<CartExtendedResponse, CartSession>().ReverseMap();
            CreateMap<CartUserResponse, CartUser>().ReverseMap();
        }
    }
}