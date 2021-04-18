using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure.Data;
using Core.Interfaces;
using System.Collections.Generic;
using Core.Specifications;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

         private readonly IGenericRepository<Product> _productsRepository;
         private readonly IGenericRepository<ProductType> _productTypesRepository;
         
         private readonly IGenericRepository<ProductBrand> _productBrandsRepository;
        public ProductsController(IGenericRepository<Product> productsRepository,
                                  IGenericRepository<ProductBrand> productBrandsRepository,
                                  IGenericRepository<ProductType> productTypesRepository
                                 )
        {
            _productsRepository=productsRepository;
            _productBrandsRepository=productBrandsRepository;
            _productTypesRepository = productTypesRepository;
        }

    [HttpGet]
    public async Task<ActionResult<Product>> GetProducts()
    {
        var spec= new ProductsWithTypesAndBrandsSpecification();
        var prodcuts = await _productsRepository.ListAsync(spec);
        return Ok(prodcuts);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var spec= new ProductsWithTypesAndBrandsSpecification(id);
        return await _productsRepository.GetEntityWithSpec(spec);

    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandsRepository.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypesRepository.ListAllAsync());
    }

}
}