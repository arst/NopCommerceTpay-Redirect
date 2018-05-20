using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.TPay
{
    public class TPayPaymentSettings : ISettings
    {
        public int MerchantID
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
    }
}
