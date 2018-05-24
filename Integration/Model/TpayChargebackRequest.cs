using Newtonsoft.Json;

namespace Nop.Plugin.Payments.Tpay.Integration.Model
{
    class TpayChargebackRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("api_password")]
        public string ApiPassword { get; set; }
    }
}
