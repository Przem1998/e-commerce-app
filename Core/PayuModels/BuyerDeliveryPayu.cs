using Newtonsoft.Json;

namespace Core.PayuModels
{
    public class BuyerDeliveryPayu
    {
        [JsonProperty(PropertyName = "street")]
        public string Street { get; set; }
        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }
    }
}