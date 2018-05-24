using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.Tpay.Infrastructure;
using Nop.Plugin.Payments.Tpay.Integration.Model;
using Nop.Plugin.Payments.TPay.Models;
using Nop.Services.Configuration;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Payments.TPay.Controllers
{
    [SuppressMessage("ReSharper", "Mvc.ViewNotResolved")]
    public class TpayPaymentController : BasePaymentController
    {
        private const string ConfigurationViewPath = "~/Plugins/Payments.TPay/Views/Configure.cshtml";

        private const string PaymentInfoViewPath = "~/Plugins/Payments.TPay/Views/PaymentInfo.cshtml";

        private readonly ISettingService settingService;

        private readonly IPaymentService paymentService;

        private readonly IOrderService orderService;

        private readonly IOrderProcessingService orderProcessingService;

        private readonly TpayPaymentSettings tPayPaymentSettings;

        private readonly PaymentSettings paymentSettings;

        private readonly HashSet<string> availableIPsTable;

        public TpayPaymentController(ISettingService settingService, IPaymentService paymentService, IOrderService orderService, IOrderProcessingService orderProcessingService, TpayPaymentSettings tPayPaymentSettings, PaymentSettings paymentSettings)
        {
            this.settingService = settingService;
            this.paymentService = paymentService;
            this.orderService = orderService;
            this.orderProcessingService = orderProcessingService;
            this.tPayPaymentSettings = tPayPaymentSettings;
            this.paymentSettings = paymentSettings;
            availableIPsTable = String.IsNullOrEmpty(tPayPaymentSettings.TPayNotifierIPs)
                ? new HashSet<string>()
                : new HashSet<string>(tPayPaymentSettings.TPayNotifierIPs.Split(','));
        }

        [AdminAuthorize, ChildActionOnly]
        public ActionResult Configure()
        {
            ConfigurationViewModel model = new ConfigurationViewModel();
            model.AdditionalFee = tPayPaymentSettings.AdditionalFee;
            model.IncludeShippingMethodInDescription = tPayPaymentSettings.IncludeShippingMethodInDescription;
            model.MerchantId = tPayPaymentSettings.MerchantId;
            model.MerchantSecret = tPayPaymentSettings.MerchantSecret;
            model.ApiKey = tPayPaymentSettings.ApiKey;
            model.ApiPassword = tPayPaymentSettings.ApiPassword;
            model.ResultEmail = tPayPaymentSettings.ResultEmail;
            model.ReturnErrorUrl = tPayPaymentSettings.ReturnErrorUrl;
            model.ReturnUrl = tPayPaymentSettings.ReturnUrl;
            model.Language = tPayPaymentSettings.Language;
            model.TPayNotifierIPs = tPayPaymentSettings.TPayNotifierIPs;

            return View(ConfigurationViewPath, model);
        }

        [AdminAuthorize, ChildActionOnly, HttpPost]
        public ActionResult Configure(ConfigurationViewModel model)
        {
            ActionResult result;
            if (!ModelState.IsValid)
            {
                result = Configure();
            }
            else
            {
                tPayPaymentSettings.IncludeShippingMethodInDescription = model.IncludeShippingMethodInDescription;
                tPayPaymentSettings.AdditionalFee = model.AdditionalFee;
                tPayPaymentSettings.MerchantId = model.MerchantId;
                tPayPaymentSettings.MerchantSecret = model.MerchantSecret;
                tPayPaymentSettings.ApiKey = model.ApiKey;
                tPayPaymentSettings.ApiPassword = model.ApiPassword;
                tPayPaymentSettings.ResultEmail = model.ResultEmail;
                tPayPaymentSettings.ReturnErrorUrl = model.ReturnErrorUrl;
                tPayPaymentSettings.ReturnUrl = model.ReturnUrl;
                tPayPaymentSettings.Language = model.Language;
                tPayPaymentSettings.TPayNotifierIPs = model.TPayNotifierIPs;
                settingService.SaveSetting(tPayPaymentSettings);
                result = View(ConfigurationViewPath, model);
            }
            return result;
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            return View(PaymentInfoViewPath);
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
        public string Return(TpayNotification notification)
        {
            if (!IsNotificationValid(notification) || !IsTpayPaymentProcessorEnabled())
                return GenerateErrorResponse("Invalid request");

            var localOrderNumber = Convert.ToInt32(notification.tr_crc);
            var order = orderService.GetOrderById(localOrderNumber);
                
            if (IsSuccessfullTransaction(notification) && 
                orderProcessingService.CanMarkOrderAsPaid(order))
            {
                orderProcessingService.MarkOrderAsPaid(order);
            }
            else
            {
                order.CaptureTransactionResult = notification.tr_error;
                orderService.UpdateOrder(order);
            }

            return TpayTransactionStatus.Success;

        }

        private string GenerateCheckSum(TpayNotification notification)
        {
            return Md5HashManager.GetMd5Hash($"{tPayPaymentSettings.MerchantId}{notification.tr_id}{notification.tr_amount}{notification.tr_crc}{tPayPaymentSettings.MerchantSecret}");
        }

        private static string GenerateErrorResponse(string message)
        {
            return $"{TpayTransactionStatus.Error} - {message}";
        }

        private bool IsTpayPaymentProcessorEnabled()
        {
            return paymentService.LoadPaymentMethodBySystemName("Payments.TPay") is TpayPaymentProcessor processor && processor.IsPaymentMethodActive(paymentSettings) && processor.PluginDescriptor.Installed;
        }

        private bool IsNotificationValid(TpayNotification notification) => availableIPsTable.Contains(Request.UserHostAddress) && notification.Md5Sum.Equals(GenerateCheckSum(notification), StringComparison.OrdinalIgnoreCase);

        private static bool IsSuccessfullTransaction(TpayNotification notification)
        {
            return notification.tr_status.Equals(TpayTransactionStatus.Success, StringComparison.OrdinalIgnoreCase) && 
                !notification.tr_error.Equals(TpayTransactionStatus.Absent, StringComparison.OrdinalIgnoreCase);
        }
    }
}