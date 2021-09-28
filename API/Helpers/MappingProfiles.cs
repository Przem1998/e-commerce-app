using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
//using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDtos>()
                .ForMember(d=>d.ProductSize,o=>o.MapFrom(s=>s.ProductSize.Size))
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
                .ForMember(d =>d.PictureUrl,o=>o.MapFrom<ProductURLResolver>());
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto,CustomerBasket>();
            CreateMap<BasketItemDto,BasketItem>();
            CreateMap<AddressDto, Core.OrderAggregate.Address>();
        }
    }
}