using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.Tpay.Integration.Model;
using Nop.Plugin.Payments.TPay.Controllers;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Routing;
using Nop.Plugin.Payments.Tpay.Integration;

namespace Nop.Plugin.Payments.TPay
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class TpayPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly ITpayPaymentManager paymentManager;

        private readonly TpayPaymentSettings tPayPaymentSettings;

        private readonly ISettingService settingService;

        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => true;

        public bool SupportRefund => true;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        public bool SkipPaymentInfo => false;

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart) => false;

        public string PaymentMethodDescription => "TPay";

        public TpayPaymentProcessor(TpayPaymentSettings tPayPaymentSettings, ISettingService settingService, ITpayPaymentManager paymnentManager)
        {
            this.tPayPaymentSettings = tPayPaymentSettings;
            this.settingService = settingService;
            this.paymentManager = paymnentManager;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest) => new ProcessPaymentResult();

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var createTransactionResult = paymentManager.CreatePayment(postProcessPaymentRequest.Order);

            if (!string.IsNullOrEmpty(createTransactionResult.Err))
            {
                throw new NopException(createTransactionResult.Err);
            }
            postProcessPaymentRequest.Order.CaptureTransactionId = createTransactionResult.Title;
            postProcessPaymentRequest.Order.CaptureTransactionResult = createTransactionResult.Desc;
            HttpContext.Current.Response.Redirect(createTransactionResult.Url);
            HttpContext.Current.Response.Flush();
        }
        
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return tPayPaymentSettings.AdditionalFee;
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            CapturePaymentResult result = new CapturePaymentResult();
            result.AddError("Capture method not supported");
            return result;
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            RefundPaymentResult result = new RefundPaymentResult();
            var chargebackResult = paymentManager.CreateChargeback(refundPaymentRequest.Order.CaptureTransactionId, refundPaymentRequest.AmountToRefund, refundPaymentRequest.IsPartialRefund);

            if (chargebackResult.Result != TpayChargebackResult.Correct)
            {
                result.Errors.Add(chargebackResult.Error);
            }
            else
            {
                result.NewPaymentStatus = refundPaymentRequest.IsPartialRefund
                    ? PaymentStatus.PartiallyRefunded
                    : PaymentStatus.Refunded;
            }

            return result;
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            VoidPaymentResult result = new VoidPaymentResult();
            result.AddError("Void method not supported");
            return result;
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            ProcessPaymentResult result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            CancelRecurringPaymentResult result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            return order.PaymentStatus == PaymentStatus.Pending && 
                   (DateTime.UtcNow - order.CreatedOnUtc).TotalMinutes >= 1.0;
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "TpayPayment";
            routeValues = new RouteValueDictionary
            {
                {
                    "Namespaces",
                    "Nop.Plugin.Payments.TPay.Controllers"
                },
                {
                    "area",
                    null
                }
            };
        }

        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "TpayPayment";
            routeValues = new RouteValueDictionary
            {
                {
                    "Namespaces",
                    "Nop.Plugin.Payments.TPay.Controllers"
                },
                {
                    "area",
                    null
                }
            };
        }

        public Type GetControllerType()
        {
            return typeof(TpayPaymentController);
        }

        public override void Install()
        {
            TpayPaymentSettings settings = new TpayPaymentSettings
            {
                MerchantId = 0,
                MerchantSecret = String.Empty,
                IncludeShippingMethodInDescription = false,
                AdditionalFee = 0
            };

            settingService.SaveSetting(settings);
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.Instructions", "");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.RedirectionTip", "You will be redirected to TPay site to complete the order.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.MerchantID", "Merchant ID");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.MerchantId.Hint", "Enter Merchant ID provided by TPay.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.MerchantSecret", "Merchant Secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.MerchantSecret.Hint", "Merchant Secret generated by you.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.IncludeShippingMethodInDescription", "Include shipping method in transaction description");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.IncludeShippingMethodInDescription.Hint", "Include shipping method in transaction description");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.AdditionalFee", "Additional Fee");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.AdditionalFee.Hint", "Enter Additional.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ApiPassword", "Api Password");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ApiPassword.Hint", "Enter api password");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ResultEmail", "Result email");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ResultEmail.Hint", "Enter result email");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ReturnErrorUrl", "Return error url");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ReturnErrorUrl.Hint", "Enter return error url");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ReturnUrl", "Return url");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ReturnUrl.Hint", "Enter return url");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ApiKey", "Api Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.ApiKey.Hint", "Enter api key");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.Language", "Language");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.Language.Hint", "Language for Tpay interface when user is redirected");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.NotifierIPs", "List of IPs for Notifications");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.TPay.NotifierIPs.Hint", "List of IPs separated by comma from which application will be allowed to receive Notifications");
            base.Install();
        }

        public override void Uninstall()
        {
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.Instructions");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.RedirectionTip");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.MerchantID");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.MerchantId.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.MerchantSecret");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.MerchantSecret.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.IncludeShippingMethodInDescription");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.IncludeShippingMethodInDescription.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.AdditionalFee");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.AdditionalFee.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ApiPassword");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ApiPassword.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ResultEmail");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ResultEmail.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ReturnErrorUrl");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ReturnErrorUrl.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ReturnUrl");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ReturnUrl.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ApiKey");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.ApiKey.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.Language");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.Language.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.NotifierIPs");
            this.DeletePluginLocaleResource("Plugins.Payments.TPay.NotifierIPs.Hint");
            base.Uninstall();
        }
    }
}