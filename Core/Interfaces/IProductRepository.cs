using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        
        Task<IReadOnlyList<SystemType>> GetAllSystemTypesAsync();

        Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync();
        Task<Product> AddProduct(Product product);
        Task<bool> DeleteProduct(int id);

    }
}