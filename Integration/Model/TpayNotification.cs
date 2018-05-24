using System;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.Tpay.Integration.Model
{
    public class TpayNotification
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tran_id")]
        public string TranId { get; set; }

        [JsonProperty("tr_date")]
        public DateTime TrDate { get; set; }

        [JsonProperty("tr_crc")]
        public string TrCrc { get; set; }

        [JsonProperty("tr_amount")]
        public string TrAmount { get; set; }

        [JsonProperty("tr_paid")]
        public string TrPaid { get; set; }

        [JsonProperty("tr_desc")]
        public string TrDesc { get; set; }

        [JsonProperty("tr_status")]
        public string TrStatus { get; set; }

        [JsonProperty("tr_error")]
        public string TrError { get; set; }

        [JsonProperty("tr_email")]
        public string TrEmail { get; set; }

        [JsonProperty("md5sum")]
        public string Md5Sum { get; set; }

        [JsonProperty("test_mode")]
        public int TestMode { get; set; }

        [JsonProperty("wallet")]
        public string Wallet { get; set; }
    }
}
