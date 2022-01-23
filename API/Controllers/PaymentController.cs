using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.PayuModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class PaymentController : BaseAPIController
    {
        private readonly IPayuService _paymentService;
        private readonly IMapper _mapper;
        public PaymentController(IPayuService paymentService, IMapper mapper)
        {
            _mapper = mapper;
            _paymentService = paymentService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto dto)
        {
            var order = new Order();
            order.BuyerEmail = HttpContext.User.RetrieveEmailFromPrincipal();
            order.ShipToAddress = _mapper.Map<AddressDto, Address>(dto.ShipToAddress);


            using (HttpClient client = new HttpClient())
            {
                string token = await _paymentService.GetBearer();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var orderToPay = await _paymentService.CreateOrder(order, dto.BasketId);
                var content = new StringContent(JsonConvert.SerializeObject(orderToPay), Encoding.UTF8, "application/json");
                var respone = client.PostAsync(_paymentService.GetOrderUrlPayu(), content);

                if (respone.Result.IsSuccessStatusCode)
                {
                    return Ok(respone.Result.RequestMessage.RequestUri);
                }
                else return BadRequest(respone.Result);


            }
        }

    }
}
