using System.Linq;
using API.Errors;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API.Extansions
{
    public static class ApplicationServicesExtansions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) //extending IServiceCollectoon
        {
           services.AddScoped<ITokenService, TokenService>();
           services.AddScoped<IOrderService, OrderService>();
           services.AddScoped<IPayuService, PayuService>();
           services.AddScoped<IUnitOfWork, UnitOfWork>();
           services.AddScoped<IProductRepository, ProductRepository>();
           services.AddScoped<IBasketRepository,BasketRepository>();
            services.TryAddScoped<SignInManager<AppUser>>();
           services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
          
            services.Configure<ApiBehaviorOptions>(options=>
            {
                options.InvalidModelStateResponseFactory=actionContext=>
                {
                    var errors = actionContext.ModelState
                                .Where(e => e.Value.Errors.Count>0)
                                .SelectMany(x=> x.Value.Errors)
                                .Select(x=> x.ErrorMessage).ToArray();

                      var errorResponse= new APIValidationErrorResponse()
                        {
                            Errors=errors
                        };
                
                return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }   
    }
}