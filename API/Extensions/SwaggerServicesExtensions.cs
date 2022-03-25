using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extansions
{
    public static class SwaggerServicesExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlumberShop", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                  Description = "JWT Auth Bearer Scheme",
                  Name = "Authorization",
                  In = ParameterLocation.Header,
                  Type = SecuritySchemeType.Http,
                  Scheme = "bearer",
                  Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                  }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequiremnt = new OpenApiSecurityRequirement{{securitySchema, new[]{"Bearer"}}};
                c.AddSecurityRequirement(securityRequiremnt) ;
            });
          
          return services;
        }
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlumberShop"));
        
          return app;
        }
    }
}