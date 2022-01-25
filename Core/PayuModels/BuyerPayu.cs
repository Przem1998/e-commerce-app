using Newtonsoft.Json;

namespace Core.PayuModels
{
    public class BuyerPayu
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
        [JsonProperty(PropertyName = "buyerDelivery")]
        public BuyerDeliveryPayu BuyerDelivery { get; set; }

    }
}