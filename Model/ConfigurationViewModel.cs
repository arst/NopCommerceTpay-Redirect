using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;

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
    }
}