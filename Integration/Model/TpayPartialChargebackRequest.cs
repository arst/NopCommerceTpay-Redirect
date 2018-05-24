using Newtonsoft.Json;

namespace Nop.Plugin.Payments.Tpay.Integration.Model
{
    class TpayPartialChargebackRequest : TpayChargebackRequest
    {
        [JsonProperty("chargeback_amount")]
        public decimal Amount { get; set; }
    }
}
