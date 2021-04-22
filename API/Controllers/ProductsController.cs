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
using API.Errors;
using Microsoft.AspNetCore.Http;

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
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //Swagger doesn't see status
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDtos>>> GetProducts(string sort, int? brandId, int? typeId)
    {
        var spec= new ProductsWithTypesAndBrandsSpecification(sort,brandId,typeId);
    
        var prodcuts = await _productsRepository.ListAsync(spec);
        return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDtos>>(prodcuts));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //Swagger doesn't see status
    public async Task<ActionResult<ProductToReturnDtos>> GetProduct(int id)
    {
        var spec= new ProductsWithTypesAndBrandsSpecification(id);
        var product= await _productsRepository.GetEntityWithSpec(spec);

        if(product == null) return NotFound(new ApiResponse(404));
        return _mapper.Map<Product,ProductToReturnDtos>(product);

    }

    [HttpGet("brands")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //Swagger doesn't see status
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandsRepository.ListAllAsync());
    }

    [HttpGet("types")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //Swagger doesn't see status
     public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypesRepository.ListAllAsync());
    }

}
}