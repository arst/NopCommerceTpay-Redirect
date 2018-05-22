using Newtonsoft.Json;

namespace Nop.Plugin.Payments.Tpay.Integration.Model
{
    public class TpayTransaction
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("crc")]
        public int Crc { get; set; }

        [JsonProperty("md5sum")]
        public string Md5Sum { get; set; }

        [JsonProperty("online")]
        public int Online { get; set; }

        [JsonProperty("group")]
        public int Group { get; set; }

        [JsonProperty("result_url")]
        public string ResultUrl { get; set; }

        [JsonProperty("result_email")]
        public string ResultEmail { get; set; }

        [JsonProperty("merchant_description")]
        public string MerchantDescription { get; set; }

        [JsonProperty("custom_description")]
        public string CustomDescription { get; set; }

        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }

        [JsonProperty("return_error_url")]
        public string ReturnErrorUrl { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("accept_tos")]
        public int AcceptTos { get; set; }

        [JsonProperty("api_password")]
        public string ApiPassword { get; set; }
    }
}
