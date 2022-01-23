using Newtonsoft.Json;

namespace Core.PayuModels
{
    public class PayMethod
    {
        [JsonProperty(PropertyName = "payMethod")]
        public string Type { get; set; }
    }
}