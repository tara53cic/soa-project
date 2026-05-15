using AutoMapper;
using PurchaseService.Models;

using AutoMapper;
using PurchaseService.Models;
using PurchaseService.DTOs;

namespace PurchaseService.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<TourPurchaseToken, TourPurchaseTokenDto>();

            CreateMap<OrderItemDto, OrderItem>();
        }
    }
}
