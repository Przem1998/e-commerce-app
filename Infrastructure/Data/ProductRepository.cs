using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<SystemType>> GetAllSystemTypesAsync()
        {
           return await _context.SystemTypes.ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
         //   var products = _context.Products.OrderBy

            return  await _context.Products
                        .Include(p=>p.SystemType)
                        .Include(p=>p.ProductType)
                        .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync()
        {
           return  await _context.ProductTypes.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            
        
            return await _context.Products
                        .Include(p=>p.SystemType)
                        .Include(p=>p.ProductType)
                        .FirstOrDefaultAsync(p=>p.Id==id);
        }

        public Task<Product> AddProduct(Product product)
        {
            
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteProduct(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}