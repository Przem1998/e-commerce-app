using System.Threading.Tasks;
using API.Dtos;
using Core.Interfaces;
//using Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.OrderAggregate;
using AutoMapper;
using System.Linq;
using System.Security.Claims;
using API.Extensions;
using API.Errors;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : BaseAPIController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email,orderDto.DeliveryMethodId,orderDto.BasketId, address);
            if(order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
            return order;
        }
    }
}