using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Payments.TPay.Models
{
    public class ConfigurationViewModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Payments.TPay.MerchantId")]
        public int MerchantId
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.MerchantSecret")]
        public string MerchantSecret
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.IncludeShippingMethodInDescription")]
        public bool IncludeShippingMethodInDescription
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.AdditionalFee")]
        public int AdditionalFee
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.ApiPassword")]
        public string ApiPassword
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.ResultEmail")]
        public string ResultEmail
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.ReturnErrorUrl")]
        public string ReturnErrorUrl
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.ReturnUrl")]
        public string ReturnUrl
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.ApiKey")]
        public string ApiKey
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugins.Payments.TPay.Language")]
        public string Language
        {
            get;
            set;
        }
    }
}