using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.TPay
{
    public class TpayPaymentSettings : ISettings
    {
        public int MerchantId
        {
            get;
            set;
        }

        public string MerchantSecret
        {
            get;
            set;
        }

        public bool IncludeShippingMethodInDescription
        {
            get;
            set;
        }

        public int AdditionalFee
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }

        public string ApiPassword
        {
            get;
            set;
        }

        public string ResultEmail
        {
            get;
            set;
        }

        public string ReturnErrorUrl
        {
            get;
            set;
        }

        public string ReturnUrl
        {
            get;
            set;
        }

        public string ApiKey
        {
            get;
            set;
        }

        public string TPayNotifierIPs
        {
            get;
            set;
        }
    }
}
