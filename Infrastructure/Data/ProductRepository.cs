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

        public async Task<IReadOnlyList<ProductSize>> GetAllProductSizesAsync()
        {
           return await _context.ProductSizes.ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
         //   var products = _context.Products.OrderBy

            return  await _context.Products
                        .Include(p=>p.ProductSize)
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
                        .Include(p=>p.ProductSize)
                        .Include(p=>p.ProductType)
                        .FirstOrDefaultAsync(p=>p.Id==id);
        }
    }
}