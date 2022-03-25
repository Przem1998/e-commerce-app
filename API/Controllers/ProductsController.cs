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


         private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(
                                  IMapper mapper,
                                  IUnitOfWork unitOfWork
                                 )
        {
        
            _mapper=mapper;
            _unitOfWork = unitOfWork;
        }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDtos>>> GetProducts([FromQuery]ProductSpecParams productParams)
    {
        var spec= new ProductsWithTypesAndSystemsSpecification(productParams);
        var countSpec= new ProductWithFiltersForCountSpecification(productParams);
        var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
        var prodcuts = await _unitOfWork.Repository<Product>().ListAsync(spec);
        var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDtos>>(prodcuts);
        return Ok(new Pagination<ProductToReturnDtos>(productParams.PageIndex,productParams.PageSize,totalItems,data));
    }
    
       
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDtos>> GetProduct(int id)
    {
        var product= await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if(product == null) return NotFound(new ApiResponse(404));
        return _mapper.Map<Product,ProductToReturnDtos>(product);
    }

    [HttpGet("systems")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    public async Task<ActionResult<IReadOnlyList<SystemType>>> GetProductBrands()
    {
        return Ok(await _unitOfWork.Repository<SystemType>().ListAllAsync());
    }

    [HttpGet("types")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
     public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _unitOfWork.Repository<ProductType>().ListAllAsync());
    }
    }
}
