using System;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
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
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Product> _productsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
                                ITokenService tokenService, IMapper mapper, IGenericRepository<Product> productsRepository,
                                IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productsRepository = productsRepository;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenService = tokenService;
            this._mapper = mapper;
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
       



     
    }
}