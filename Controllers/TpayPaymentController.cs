using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
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
        private readonly ISettingService settingService;

        private readonly IPaymentService paymentService;

        private readonly IOrderService orderService;

        private readonly IOrderProcessingService orderProcessingService;

        private readonly TpayPaymentSettings tPayPaymentSettings;

        private readonly PaymentSettings paymentSettings;

        public TpayPaymentController(ISettingService settingService, IPaymentService paymentService, IOrderService orderService, IOrderProcessingService orderProcessingService, TpayPaymentSettings tPayPaymentSettings, PaymentSettings paymentSettings)
        {
            this.settingService = settingService;
            this.paymentService = paymentService;
            this.orderService = orderService;
            this.orderProcessingService = orderProcessingService;
            this.tPayPaymentSettings = tPayPaymentSettings;
            this.paymentSettings = paymentSettings;
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

            return View("~/Plugins/Payments.TPay/Views/Configure.cshtml", model);
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
                settingService.SaveSetting(tPayPaymentSettings);
                result = View("~/Plugins/Payments.TPay/Views/Configure.cshtml", model);
            }
            return result;
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            return View("~/Plugins/Payments.TPay/Views/PaymentInfo.cshtml");
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
            TpayPaymentProcessor processor = paymentService.LoadPaymentMethodBySystemName("Payments.TPay") as TpayPaymentProcessor;
            if (processor == null || 
                !processor.IsPaymentMethodActive(paymentSettings) || 
                !processor.PluginDescriptor.Installed)
            {
                throw new NopException("TPay payments module cannot be loaded");
            }
            int localOrderNumber = Convert.ToInt32(notification.TrCrc);
            Order order = orderService.GetOrderById(localOrderNumber);
            if (notification.TrStatus.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                if (orderProcessingService.CanMarkOrderAsPaid(order))
                {
                    order.AuthorizationTransactionId = notification.TranId;
                    orderService.UpdateOrder(order);
                    orderProcessingService.MarkOrderAsPaid(order);
                }
            }
            else
            {
                if (orderProcessingService.CanCancelOrder(order))
                {
                    order.AuthorizationTransactionId = notification.TranId;
                    order.AuthorizationTransactionResult = notification.TrError;
                    orderService.UpdateOrder(order);
                    orderProcessingService.CancelOrder(order, true);
                }
            }
            
            return "TRUE";
        }
    }
}