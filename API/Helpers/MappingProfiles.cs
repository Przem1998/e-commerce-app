using API.Dtos;
using AutoMapper;
using Core.Entities;

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
        }
    }
}