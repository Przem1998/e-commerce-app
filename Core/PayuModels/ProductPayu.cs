using Newtonsoft.Json;

namespace Core.PayuModels
{
    public class ProductPayu
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "unitPrice")]
        public string UnitPrice { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public string Quantity { get; set; }
    }
}