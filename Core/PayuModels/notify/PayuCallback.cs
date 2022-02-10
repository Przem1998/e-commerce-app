using Newtonsoft.Json;

namespace Core.PayuModels
{
    public class PayuCallback
    {
        [JsonProperty(PropertyName = "order")]
        public OrderPayuRaport Order { get; set; }
        [JsonProperty(PropertyName = "localReceiptDateTime")]
        public string LocalReceiptDateTime { get; set; }

    }
}