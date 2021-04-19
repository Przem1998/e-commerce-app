using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class ProductURLResolver : IValueResolver<Product, ProductToReturnDtos, string>
    {
        private readonly IConfiguration _config;
        public ProductURLResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDtos destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"]+source.PictureUrl;
            }
            return null;
        }
    }
}