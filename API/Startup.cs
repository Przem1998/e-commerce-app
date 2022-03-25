using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using API.Helpers;
using API.Middleware;
using API.Extansions;
using StackExchange.Redis;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

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
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            
            services.AddDbContext<StoreContext>(x=> x.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
            services.AddDbContext<AppIdentityDbContext>(x =>{
                x.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            });
            services.AddSingleton<IConnectionMultiplexer>(c=>{
                var configuration=ConfigurationOptions.Parse(_config.GetConnectionString("Redis"),
                true);
                return ConnectionMultiplexer.Connect(configuration);
            });
            
            services.AddApplicationServices();
            services.AddIdentityServices(_config );
            services.AddSwaggerDocumentation();
            services.AddCors(opt=>{
                opt.AddPolicy("CorsPolicy", policy=>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //middlware
        {
            app.UseMiddleware<ExceptionMiddleware>(); //report exceptons
       
            if (env.IsDevelopment()) //development mode
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); //middleware this move to error controller and return json result error 
            
            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseStaticFiles();
            
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            
            
            app.UseSwaggerDocumentation();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run(async(context)=>{
                await context.Response.WriteAsync("Could not find anything");
            });
        }
    }
}
