using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.PayuModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class PaymentController : BaseAPIController
    {
        private readonly IPayuService _paymentService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IOrderService _orderService;
        private readonly ILogger<IPayuService> _loggerPayu;
        public PaymentController(IPayuService paymentService, IMapper mapper, IConfiguration config, IOrderService orderService, ILogger<IPayuService> loggerPayu )
        {
            _loggerPayu = loggerPayu;
            _mapper = mapper;
            _config = config;
            _orderService = orderService;
            _paymentService = paymentService;
        }
        [HttpPost("createOrder")]
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
                if (respone.Result.IsSuccessStatusCode) return Ok(respone.Result.RequestMessage.RequestUri);
                else return BadRequest(respone.Result);
            }
        }
        [HttpPost("notify")]
        // [Authorize]
        public async Task<IActionResult> PayuCallback(PayuCallback orderPayu)
        {
            var openPayuHeader = Request.Headers["OpenPayu-Signature"].ToString();
            var openPayuHeaderParts = openPayuHeader.Split(';');
            var incoming_signature = openPayuHeaderParts.Where(s=> s.StartsWith("signature=")).Single().Substring(10);
            // //get cipher method
           var hashAlgorithm = openPayuHeaderParts.Where(s=> s.StartsWith("algorithm=")).Single().Substring(10);

             string concatenatedContent= incoming_signature+ _config["PayuSettings:SecondKeyMD5"];
             string expectedSignature = _paymentService.GetSignature(concatenatedContent,hashAlgorithm);
            
            if(expectedSignature!=incoming_signature) 
                return BadRequest(new ApiResponse(500));

            //identify order
            int orderId = int.Parse(orderPayu.Order.ExtOrderId.Substring(10));

            if (await _orderService.IsOrderComplitedOrCanceled(orderId))
            {
                _loggerPayu.LogInformation("Brak zobowiązań co do płatności"); 
                return Ok(new ApiResponse(200));
            }
            
            switch(orderPayu.Order.Status)
            {
                case "COMPLETED": _loggerPayu.LogInformation("Płatność dokonana pomyślnie"); break;
                case "CANCELED": _loggerPayu.LogInformation("Płatność anulowano"); break;
                case "ERROR": _loggerPayu.LogError("Błąd w czasie płatności"); break;
                default: _loggerPayu.LogError("Błąd nieznany"); break;
            }
            _paymentService.AddToRaport("Wywołano callback PayU: "+JsonConvert.SerializeObject(orderPayu));
            return Ok( _mapper.Map<OrderToReturnDto>(await _orderService.ChangeOrderStatus(orderId, orderPayu.Order.Status)));
        }
    }
}