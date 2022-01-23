using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Newtonsoft.Json;

namespace API.Controllers
{
    public class PaymentController : BaseAPIController
    {
        private readonly IPayuService _paymentService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IOrderService _orderService;

        public PaymentController(IPayuService paymentService, IMapper mapper, IConfiguration config, IOrderService orderService)
        {
            _mapper = mapper;
            _config = config;
            _orderService = orderService;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PayuCallback(PayuCallback orderPayu)
        {
            //Validate signature
            var input = new StreamReader(Request.Scheme).ReadToEnd();
            var openPayuHeader = Request.Headers["OpenPayu-Signature"];
            var openPayuHeaderParts = openPayuHeader.ToArray();
            //Isolate signature
            var signature = openPayuHeaderParts.Where(s=> s.StartsWith("signature=")).Single().Substring(10);
            //get cipher method
            var hashAlgorithm = openPayuHeader.Where(s=> s.StartsWith("algorithm=")).Single().Substring(10);
    
            string concatenatedContent= input+ _config["PayuSettings:SecondKeyMD5"];
            string expectedSignature = _paymentService.GetSignature(concatenatedContent,hashAlgorithm);
            if(expectedSignature!=signature) 
                return BadRequest(new ApiResponse(500));
            
            //identify order
            int orderId = int.Parse(orderPayu.Order.ExtOrderId);
            
            if( await _orderService.IsOrderComplitedOrCanceled(orderId))
                return Ok(new ApiResponse(200));
            
            await _orderService.ChangeOrderStatus(orderId,orderPayu.Order.Status);
            
            return Ok("Wywołano callback payu "+JsonConvert.SerializeObject(orderPayu));
        }

    }
}
