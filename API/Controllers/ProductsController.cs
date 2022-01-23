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
using API.Helpers;
using System;

namespace API.Controllers
{


    public class ProductsController : BaseAPIController
    {

         private readonly IGenericRepository<Product> _productsRepository;
         private readonly IGenericRepository<ProductType> _productTypesRepository;
         
         private readonly IGenericRepository<ProductSize> _productSizesRepository;
         private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IGenericRepository<Product> productsRepository,
                                  IGenericRepository<ProductSize> productSizesRepository,
                                  IGenericRepository<ProductType> productTypesRepository,
                                  IMapper mapper,
                                  IUnitOfWork unitOfWork
                                 )
        {
            _productsRepository=productsRepository;
            _productSizesRepository=productSizesRepository;
            _productTypesRepository = productTypesRepository;
            _mapper=mapper;
            _unitOfWork = unitOfWork;
        }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //Swagger doesn't see status
    public async Task<ActionResult<Pagination<ProductToReturnDtos>>> GetProducts([FromQuery]ProductSpecParams productParams)
    {
        var spec= new ProductsWithTypesAndSizesSpecification(productParams);
    
        var countSpec= new ProductWithFiltersForCountSpecification(productParams);

        var totalItems = await _productsRepository.CountAsync(countSpec);

        var prodcuts = await _productsRepository.ListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDtos>>(prodcuts);


        return Ok(new Pagination<ProductToReturnDtos>(productParams.PageIndex,productParams.PageSize,totalItems,data));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //Swagger doesn't see status
    public async Task<ActionResult<ProductToReturnDtos>> GetProduct(int id)
    {
        var spec= new ProductsWithTypesAndSizesSpecification(id);
        var product= await _productsRepository.GetEntityWithSpec(spec);

        if(product == null) return NotFound(new ApiResponse(404));
        return _mapper.Map<Product,ProductToReturnDtos>(product);

    }

    [HttpGet("sizes")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //Swagger doesn't see status
    public async Task<ActionResult<IReadOnlyList<ProductSize>>> GetProductBrands()
    {
        return Ok(await _productSizesRepository.ListAllAsync());
    }

    [HttpGet("types")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //Swagger doesn't see status
     public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypesRepository.ListAllAsync());
    }
    
    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct(ProductDto dto)
    {
        var data = await _productsRepository.ListAllAsync();
        var id = data.Count;
        var productTypes= await _productTypesRepository.ListAllAsync();
        var productSizes= await _productSizesRepository.ListAllAsync();
        var typeId =(productTypes.Where(x=> x.Name== dto.ProductType).Select(x=>x.Id)).ToArray();
        var sizeId =(productSizes.Where(x=> x.Size== dto.ProductSize).Select(x=>x.Id)).ToArray();
        var product= new Product{
            Id= id+1,
            Name= dto.Name,
            Description= dto.Description,
            Price= dto.Price,
            PictureUrl= dto.PictureUrl,
            ProductSizeId=Convert.ToInt16(sizeId[0]),
            ProductTypeId=Convert.ToInt16(typeId[0])

        };
        _unitOfWork.Repository<Product>().Add(product);
        var result = await _unitOfWork.Complete();
        // _productsRepository.Add(product);
        return Ok(await _productsRepository.GetByIdAsync(id+1));
    }
}
}