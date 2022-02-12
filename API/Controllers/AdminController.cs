using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extansions;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseAPIController
    {




        //Dostęp do payu
        //Dodanie produktów
        //Dodanie systrmów
        //Dodanie rodzajów kształtek
        //Wszystkie zamówienia

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

         private readonly IGenericRepository<Product> _productsRepository;
         private readonly IGenericRepository<ProductType> _productTypesRepository;
         
         private readonly IGenericRepository<SystemType> _productSystemTypesRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _environment;
        private readonly IOrderService _orderService;



        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
                                ITokenService tokenService, IGenericRepository<Product> productsRepository,
                                IUnitOfWork unitOfWork, IWebHostEnvironment environment, IOrderService orderService,
                                IGenericRepository<ProductType> productTypesRepository, IGenericRepository<SystemType> productSystemTypes
                                )
        {
            _orderService = orderService;
            _environment = environment;
            _unitOfWork = unitOfWork;
            _productsRepository = productsRepository;
            _productTypesRepository = productTypesRepository;
            _productSystemTypesRepository = productSystemTypes;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        public class FileUpload
        {
            public IFormFile Files { get; set; }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        //Logowanie => sprawdzenie czy uzytkownik to administrator => krotka w db
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            if (user.Role != "ADMIN") return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("deployImage")]
        public async Task<ActionResult> UploadImage([FromForm] FileUpload objFile)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "/Images"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "/Images/");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "/Images/products/" + objFile.Files.FileName))
                    {
                        objFile.Files.CopyTo(fileStream);
                        fileStream.Flush();
                        return Ok( "/Images/products/" + objFile.Files.FileName);
                    }
                }
                else
                {
                    return  NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet("allStatus")]
        public ActionResult<IReadOnlyList<string>> GetAllStatus()
        {

            return Ok(_orderService.GetAllStatus());
        }
        [HttpPut("changeStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> OrderStatusChange(OrderToReturnDto dto)
        {
            await _orderService.ChangeOrderStatus(dto.Id, dto.Status);

            return Ok(dto.Status);
        }

    [HttpGet("productExists")]
    public async Task<ActionResult<bool>> CheckProductExists([FromQuery] string product)
    {
        var products = await _productsRepository.ListAllAsync();
          return products.Any(p => p.Name == product);    
    } 
    
    [HttpPost("addProduct")]
    public async Task<ActionResult<Product>> AddProduct(ProductDto dto)
    {
        var data = await _productsRepository.ListAllAsync();
        var id = data[data.Count-1].Id ;
        var productTypes= await _productTypesRepository.ListAllAsync();
        var systemTypes= await _productSystemTypesRepository.ListAllAsync();
        
        var typeId =(productTypes.Where(x=> x.Name== dto.ProductType).Select(x=>x.Id)).ToArray();
        var sizeId =(systemTypes.Where(x=> x.Name== dto.SystemType).Select(x=>x.Id)).ToArray();
        var product= new Product{
            Id= id+1,
            Name= dto.Name,
            Description= dto.Description,
            Price= dto.Price,
            PictureUrl= dto.PictureUrl,
            SystemTypeId=Convert.ToInt16(sizeId[0]),
            ProductTypeId=Convert.ToInt16(typeId[0])

        };
        // try{
            
        _unitOfWork.Repository<Product>().Add(product);
        var result = await _unitOfWork.Complete();
        // _productsRepository.Add(product);
           return Ok("success");
        // }
        // catch(Exception e){return BadRequest(e);}
       
     
    }
        [HttpDelete("deleteProduct")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.Complete();
            return Ok($"Usunięto produkt o id {product.Name}");
        }
        [HttpPost("addSystemType")]
        public async Task<ActionResult<SystemType>> AddSystemType(string system)
        {

            var data = await _productSystemTypesRepository.ListAllAsync();
            var id = data.Count;
            var systemType = new SystemType
            {
                Id=id+1,
                Name=system
            };
             _unitOfWork.Repository<SystemType>().Add(systemType);
             await _unitOfWork.Complete();
             return Ok(await _productSystemTypesRepository.GetByIdAsync(id+1));
        }
        [HttpDelete("deleteSystemType")]
        public async Task<ActionResult> DeleteSystemType(int systemTypeId)
        {
            
            var systemType = await _unitOfWork.Repository<SystemType>().GetByIdAsync(systemTypeId);
            _unitOfWork.Repository<SystemType>().Delete(systemType);
            await _unitOfWork.Complete();
            return Ok($"Usunięto produkt o id {systemType.Name}");
        }
        [HttpPost("addSystemType")]
        public async Task<ActionResult<SystemType>> AddProductType(string type)
        {

            var data = await _productSystemTypesRepository.ListAllAsync();
            var id = data.Count;
            var productType = new ProductType
            {
                Id=id+1,
                Name=type
            };
             _unitOfWork.Repository<ProductType>().Add(productType);
             await _unitOfWork.Complete();
             return Ok(await _productSystemTypesRepository.GetByIdAsync(id+1));
        }
        [HttpDelete("deleteSystemType")]
        public async Task<ActionResult> DeleteProductType(int productTypeId)
        {
            
            var productType = await _unitOfWork.Repository<SystemType>().GetByIdAsync(productTypeId);
            _unitOfWork.Repository<SystemType>().Delete(productType);
            await _unitOfWork.Complete();
            return Ok($"Usunięto produkt o id {productType.Name}");
        }
    }
}