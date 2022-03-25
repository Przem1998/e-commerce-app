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
using Core;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Data;
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
        private readonly StoreContext context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IOrderService _orderService;



        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
                                ITokenService tokenService, IGenericRepository<Product> productsRepository,
                                IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment environment, IOrderService orderService,
                                IGenericRepository<ProductType> productTypesRepository, IGenericRepository<SystemType> productSystemTypes, StoreContext context
                                )
        {
            _orderService = orderService;
            _environment = environment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productsRepository = productsRepository;
            _productTypesRepository = productTypesRepository;
            _productSystemTypesRepository = productSystemTypes;
            this.context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        public class FileUpload
        {
            public IFormFile Files { get; set; }
        }
       
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
            if(user== null) return new UserDto();
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
                Role=user.Role
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
        [HttpGet("allOrders")]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("ordersRaport")]
        public async Task<ActionResult<OrderRaport>> GetOrdersRaport()
        {
            var orders= await _unitOfWork.Repository<Order>().ListAllAsync();
            var amountOfOrders = orders.Count;
            var ordersValue= orders.Sum(s=> s.Subtotal);
            var statusCompletedCount=0;
            var statusCanceledCount= 0;
            var statusPendingCount=0;
            foreach(var order in orders)
            {
                if(order.Status== OrderStatus.Completed) statusCompletedCount++;
                if(order.Status== OrderStatus.Canceled) statusCanceledCount++;
                if(order.Status== OrderStatus.Pending) statusPendingCount++;
            }
            return new OrderRaport{
                AmountOfOrders= amountOfOrders,
                OrdersValue=string.Format("{0:0.00}", ordersValue),
                AmountOfCompletedOrders=statusCompletedCount,
                AmountOfPendingOrders=statusPendingCount,
                AmountOfCanceledOrders=statusCanceledCount
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
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "/Images//products/" + objFile.Files.FileName))
                    {
                        objFile.Files.CopyTo(fileStream);
                        fileStream.Flush();
                        return Ok( "//Images//products//" + objFile.Files.FileName);
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
        var data = await _unitOfWork.Repository<Product>().ListAllAsync();
        int id;
        if(data.Count==0) id=0;
        else id=data[data.Count-1].Id ;
        var productTypes= await  _unitOfWork.Repository<ProductType>().ListAllAsync();
        var systemTypes= await _unitOfWork.Repository<SystemType>().ListAllAsync();
        var productTypeId =(productTypes.Where(x=> x.Name== dto.ProductType).Select(x=>x.Id)).ToArray();
        var systemTypeId =(systemTypes.Where(x=> x.Name== dto.SystemType).Select(x=>x.Id)).ToArray();
        var product= new Product{
            Id= id+1,
            Name= dto.Name,
            Description= dto.Description,
            Price= dto.Price,
            PictureUrl= dto.PictureUrl,
            SystemTypeId=Convert.ToInt16(systemTypeId[0]),
            ProductTypeId=Convert.ToInt16(productTypeId[0])

        };
        _unitOfWork.Repository<Product>().Add(product);
        await _unitOfWork.Complete();
        return Ok(product);
    }

          // try{
             // _productsRepository.Add(product);
        // }
        // catch(Exception e){return BadRequest(e);}
        [HttpDelete("deleteProduct")]
        public async Task<ActionResult<string>> DeleteProduct(int productId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.Complete();
            return Ok("Produkt usunięto");
        }
        [HttpPost("addSystemType")]
        public async Task<ActionResult<SystemType>> AddSystemType(SystemType systemType)
        {
     
             _unitOfWork.Repository<SystemType>().Add(systemType);
             await _unitOfWork.Complete();
             return Ok(systemType);
        }
        [HttpGet("CanIDeleteSystemType/{id}")]
        public async Task<ActionResult<bool>> CanIDeleteSystemType(int id)
        {
             var products= await _unitOfWork.Repository<Product>().ListAllAsync();
            foreach(var product in products)
                if(product.SystemTypeId== id) return false;

            return true;
        }
        [HttpDelete("deleteSystemType/{id}")]
        public async Task<ActionResult> DeleteSystemType(int id)
        {
            
           var systemType = await _unitOfWork.Repository<SystemType>().GetByIdAsync(id);
            _unitOfWork.Repository<SystemType>().Delete(systemType);
            await _unitOfWork.Complete();
            return Ok($"Usunięto produkt o id {systemType.Name}");
        }
        [HttpPost("addProductType")]
        public async Task<ActionResult<SystemType>> AddProductType(ProductType productType)
        {
            _unitOfWork.Repository<ProductType>().Add(productType);
            await _unitOfWork.Complete();
             return Ok(productType);
        }
        [HttpGet("CanIDeleteProductType/{id}")]
        public async Task<ActionResult<bool>> CanIDeleteProductType(int id)
        {
            var products= await _unitOfWork.Repository<Product>().ListAllAsync();
            foreach(var product in products)
                if(product.ProductTypeId== id) return false;

            return true;
        }
        [HttpDelete("deleteProductType/{id}")]
        public async Task<ActionResult> DeleteProductType(int id)
        {
            
            var productType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
            _unitOfWork.Repository<ProductType>().Delete(productType);
            await _unitOfWork.Complete();
            return Ok($"Usunięto produkt o id {productType.Name}");
        }
    }
}