using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.PayuModels
{
    public class OrderPayuRaport
    {
        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }
        [JsonProperty(PropertyName = "extOrderId")]
        public string ExtOrderId { get; set; }
        [JsonProperty(PropertyName = "orderCreateDate")]
        public string OrderCreateDate { get; set; }
        [JsonProperty(PropertyName = "notifyUrl")]
        public string NotifyUrl { get; set; }
        [JsonProperty(PropertyName = "customerIp")]
        public string CustomerIp { get; set; }
        [JsonProperty(PropertyName = "merchantPosId")]
        public string MerchantPosId { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "currencyCode")]
        public string CurrencyCode { get; set; }
        [JsonProperty(PropertyName = "totalAmount")]
        public string TotalAmount { get; set; }
        [JsonProperty(PropertyName = "buyer")]
        public BuyerPayu Buyer { get; set; }
        [JsonProperty(PropertyName = "payMethod")]
        public PayMethod PayMethod { get; set; }
        [JsonProperty(PropertyName = "products")]
        public List<ProductPayu> Products { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

       
       

    }
}