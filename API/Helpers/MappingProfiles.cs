using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDtos>()
                .ForMember(d=>d.SystemType,o=>o.MapFrom(s=>s.SystemType.Name))
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
                .ForMember(d =>d.PictureUrl,o=>o.MapFrom<ProductURLResolver>());

        //  CreateMap<ProductDto,Product>()
        //         .ForMember(d=>d.ProductSize.Size,o=>o.MapFrom(s=>s.ProductSize))
        //         .ForMember(d=>d.ProductType.Name,o=>o.MapFrom(s=>s.ProductType));

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto,CustomerBasket>();
            CreateMap<BasketItemDto,BasketItem>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ItemOrdered.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.ItemOrdered.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemResolver>());
        }
    }
}