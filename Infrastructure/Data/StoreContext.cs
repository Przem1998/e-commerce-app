using System.Linq;
using System.Reflection;
using Core.Entities;
using Core.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext( DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if(Database.ProviderName=="Microsoft.EntityFrameworkCore.Sqlite") // sqlite doesn't tollerate decimal values.  In a sql server version will be removed
            {
                foreach(var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties= entityType.ClrType.GetProperties().Where(p=> p.PropertyType == typeof(decimal));

                    foreach(var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }
    }
}