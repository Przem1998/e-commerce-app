using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure.Data;
using Core.Interfaces;
using System.Collections.Generic;
using Core.Specifications;
using API.Dtos;
using System.Linq;
using AutoMapper;

namespace API.Controllers
{


    public class ProductsController : BaseAPIController
    {

         private readonly IGenericRepository<Product> _productsRepository;
         private readonly IGenericRepository<ProductType> _productTypesRepository;
         
         private readonly IGenericRepository<ProductBrand> _productBrandsRepository;
         private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productsRepository,
                                  IGenericRepository<ProductBrand> productBrandsRepository,
                                  IGenericRepository<ProductType> productTypesRepository,
                                  IMapper mapper
                                 )
        {
            _productsRepository=productsRepository;
            _productBrandsRepository=productBrandsRepository;
            _productTypesRepository = productTypesRepository;
            _mapper=mapper;
        }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDtos>>> GetProducts()
    {
        var spec= new ProductsWithTypesAndBrandsSpecification();
        var prodcuts = await _productsRepository.ListAsync(spec);
        return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDtos>>(prodcuts));
        }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDtos>> GetProduct(int id)
    {
        var spec= new ProductsWithTypesAndBrandsSpecification(id);
 

     var product= await _productsRepository.GetEntityWithSpec(spec);
       return _mapper.Map<Product,ProductToReturnDtos>(product);

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