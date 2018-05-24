using Newtonsoft.Json;

namespace Nop.Plugin.Payments.Tpay.Integration.Model
{
    class TpayChargebackResponse
    {
        [JsonProperty("result")]
        public TpayChargebackResult Result { get; set; }

        [JsonProperty("err")]
        public string Error { get; set; }
    }
}
