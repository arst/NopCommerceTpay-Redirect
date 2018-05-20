using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Security;
using Nop.Plugin.Payments.TPay.Model;
using Nop.Plugin.Payments.TPay.Models;
using System.Xml.Serialization;

namespace Nop.Plugin.Payments.TPay.Controllers
{
    public class TpayPaymentController : BasePaymentController
    {
        private readonly ISettingService _settingService;

        private readonly IPaymentService _paymentService;

        private readonly IOrderService _orderService;

        private readonly IOrderProcessingService _orderProcessingService;

        private readonly TPayPaymentSettings _TPayPaymentSettings;

        private readonly PaymentSettings _paymentSettings;

        private readonly ILogger _logger;

        public TpayPaymentController(ISettingService settingService, IPaymentService paymentService, IOrderService orderService, IOrderProcessingService orderProcessingService, TPayPaymentSettings PayuPaymentSettings, PaymentSettings paymentSettings, ILogger _logger)
        {
            this._settingService = settingService;
            this._paymentService = paymentService;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._TPayPaymentSettings = PayuPaymentSettings;
            this._paymentSettings = paymentSettings;
            this._logger = _logger;
        }

        [AdminAuthorize, ChildActionOnly]
        public ActionResult Configure()
        {
            ConfigurationViewModel model = new ConfigurationViewModel();
            model.AdditionalFee = this._TPayPaymentSettings.AdditionalFee;
            model.IncludeShippingMethodInDescription = this._TPayPaymentSettings.IncludeShippingMethodInDescription;
            model.MerchantId = this._TPayPaymentSettings.MerchantID;
            model.MerchantSecret = this._TPayPaymentSettings.MerchantSecret;
            return base.View("~/Plugins/Payments.TPay/Views/Configure.cshtml", model);
        }

        [AdminAuthorize, ChildActionOnly, HttpPost]
        public ActionResult Configure(ConfigurationViewModel model)
        {
            ActionResult result;
            if (!base.ModelState.IsValid)
            {
                result = this.Configure();
            }
            else
            {
                this._TPayPaymentSettings.AdditionalFee = model.AdditionalFee;
                this._TPayPaymentSettings.IncludeShippingMethodInDescription = model.IncludeShippingMethodInDescription;
                this._TPayPaymentSettings.MerchantID = model.MerchantId;
                this._TPayPaymentSettings.MerchantSecret = model.MerchantSecret;
                this._settingService.SaveSetting<TPayPaymentSettings>(this._TPayPaymentSettings, 0);
                result = base.View("~/Plugins/Payments.TPay/Views/Configure.cshtml", model);
            }
            return result;
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            return base.View("~/Plugins/Payments.TPay/Views/PaymentInfo.cshtml");
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            return new List<string>();
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        
        [ValidateInput(false)]
        public async Task<string> Return(TPayNotification notification)
        {
            TPayPaymentProcessor processor = this._paymentService.LoadPaymentMethodBySystemName("Payments.TPay") as TPayPaymentProcessor;
            if (processor == null || !PaymentExtensions.IsPaymentMethodActive(processor, this._paymentSettings) || !processor.PluginDescriptor.Installed)
            {
                throw new NopException("TPay payments module cannot be loaded");
            }
            int localOrderNumber = Convert.ToInt32(notification.tr_crc);
            Order order = this._orderService.GetOrderById(localOrderNumber);
            if (notification.tr_status.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                if (this._orderProcessingService.CanMarkOrderAsPaid(order))
                {
                    order.AuthorizationTransactionId = notification.tran_id;
                    this._orderService.UpdateOrder(order);
                    this._orderProcessingService.MarkOrderAsPaid(order);
                }
            }
            else
            {
                if (this._orderProcessingService.CanCancelOrder(order))
                {
                    order.AuthorizationTransactionId = notification.tran_id;
                    order.AuthorizationTransactionResult = notification.tr_error;
                    this._orderService.UpdateOrder(order);
                    this._orderProcessingService.CancelOrder(order, true);
                }
            }

            return "TRUE";
        }

        private bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5Hash(md5Hash, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

    }
}