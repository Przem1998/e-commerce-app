using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            
            try
            {
                if (!context.SystemTypes.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/SeedData/systems.json");
                    var brands = JsonSerializer.Deserialize<List<SystemType>>(brandsData);
                    foreach (var item in brands)
                    {
                        context.Database.BeginTransaction();
                        context.SystemTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Infrastructure/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.DeliveryMethods.Any())
                {
                    var dmData = File.ReadAllText("../Infrastructure/SeedData/delivery.json");
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    foreach (var item in methods)
                    {
                        context.DeliveryMethods.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.SystemTypes.Any())
                {
                    var logsData = File.ReadAllText("../Infrastructure/SeedData/logs.json");
                    var logs = JsonSerializer.Deserialize<List<SystemType>>(logsData);
                    foreach (var log in logs)
                    {
                        context.SystemTypes.Add(log);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
               var logger=loggerFactory.CreateLogger<StoreContextSeed>();
               logger.LogError(ex.Message); 
            }
        }
        
    }
}