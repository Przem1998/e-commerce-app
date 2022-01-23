using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.PayuModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
namespace Infrastructure.Services
{
    public class PayuService : IPayuService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public PayuService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

        public async Task<OrderPayu> CreateOrder(Order order, string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            var productPayuItems = new List<ProductPayu>();
  
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != productItem.Price) 
                item.Price = productItem.Price;

                ProductPayu productPayu = new ProductPayu {
                    Name=productItem.Name,
                    UnitPrice=((int)(productItem.Price*100)).ToString(),
                    Quantity = item.Quantity.ToString()
                };
                productPayuItems.Add(productPayu);
            }


        BuyerPayu buyer = new BuyerPayu{
            Email=order.BuyerEmail,
            Phone="795317224",
            FirstName=order.ShipToAddress.FirstName,
            LastName=order.ShipToAddress.LastName,
            Language="pl"
        };
           
         int total =(int)(basket.Items.Sum(p => (p.Price *100)*p.Quantity)+shippingPrice*100);
           return new OrderPayu {
               NotifyUrl= "https://your.eshop.com/notify",
            CustomerIp= "127.0.0.1",
            MerchantPosId= _config["PayuSettings:pos_id"],
            Description= "Zamówienie numer 1",
            CurrencyCode= "PLN",
            TotalAmount=total.ToString(),
            Buyer=buyer,
            Products=productPayuItems
            };
        }

        public async Task<string> GetBearer()
        {
            AuthDeserialize deserializeAuth;
            using(HttpClient httpClient = new HttpClient())
            {
            var data = new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id",_config["PayuSettings:client_id"]),
                new KeyValuePair<string, string>("client_secret", _config["PayuSettings:client_secret"]),
            };
            var response = httpClient.PostAsync(_config["PayuSettings:auth_url"], new FormUrlEncodedContent(data)).GetAwaiter().GetResult();
            var result = await response.Content.ReadAsStringAsync();
             deserializeAuth= JsonConvert.DeserializeObject<AuthDeserialize>(result);
            }
            return deserializeAuth.AccessToken;
        }

        public Task<CustomerBasket> UpdateBasketPrice(string basketId)
        {
            throw new System.NotImplementedException();
        }

        public string GetOrderUrlPayu() => _config["PayuSettings:order_url"];

        public string GetSignature(string input, string algorithm)
        {
           var hashAlgorithm = (HashAlgorithm)CryptoConfig.CreateFromName(algorithm);
           Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
           Byte[] hashedBytes = hashAlgorithm.ComputeHash(inputBytes);
           return BitConverter.ToString(hashedBytes).Replace("-",string.Empty).ToLower();
        }

    
    }
}