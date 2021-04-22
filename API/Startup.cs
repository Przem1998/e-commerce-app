using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Data;
using Core.Interfaces;
using API.Helpers;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API.Errors;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
        _config=config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            
            services.AddDbContext<StoreContext>(x=> x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //middlware
        {
            app.UseMiddleware<ExceptionMiddleware>(); //report exceptons
       
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
        
            // if (env.IsDevelopment()) //development mode
            // {
            // }
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); //middleware this move to error controller and return json result error 


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
