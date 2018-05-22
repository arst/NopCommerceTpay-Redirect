using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nop.Plugin.Payments.Tpay.Integration.Model
{
    public class CreateTpayTransactionResponse
    {
        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("err")]
        public string Err { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("online")]
        public Online Online { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }
    }
}
