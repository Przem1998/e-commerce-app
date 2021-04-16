using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure.Data;
using Core.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        public ProductsController(IProductRepository repository)
        {
           _repository=repository;

        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetProducts()
        {
           var prodcuts = await _repository.GetAllProductsAsync();
           return Ok(prodcuts);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
        return await _repository.GetProductByIdAsync(id);
        
        }


    }
}