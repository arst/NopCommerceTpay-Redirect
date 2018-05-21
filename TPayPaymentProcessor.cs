using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.TPay.Controllers;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Payments;
using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using System.Web.Routing;
using System.Text;
using System.Net.Http;
using Nop.Plugin.Payments.TPay;
using IO.Swagger.Api;
using IO.Swagger.Model;

namespace Nop.Plugin.Payments.TPay
{
    public class TPayPaymentProcessor : BasePlugin, IPaymentMethod, IPlugin
    {
        private readonly TPayPaymentSettings _TPayPaymentSettings;

        private readonly ISettingService _settingService;

        private readonly ICurrencyService _currencyService;

        private readonly CurrencySettings _currencySettings;

        private readonly IWebHelper _webHelper;

        private readonly ILogger _logger;

        public bool SupportCapture
        {
            get
            {
                return false;
            }
        }

        public bool SupportPartiallyRefund
        {
            get
            {
                return true;
            }
        }

        public bool SupportRefund
        {
            get
            {
                return true;
            }
        }

        public bool SupportVoid
        {
            get
            {
                return false;
            }
        }

        public RecurringPaymentType RecurringPaymentType
        {
            get
            {
                return RecurringPaymentType.NotSupported;
            }
        }

        public PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Redirection;
            }
        }

        public bool SkipPaymentInfo
        {
            get
            {
                return false;
            }
        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            return false;
        }

        public string PaymentMethodDescription => "TPay";

        public TPayPaymentProcessor(TPayPaymentSettings TPayPaymentSettings, ISettingService settingService, ICurrencyService currencyService, CurrencySettings currencySettings, IWebHelper webHelper, ILogger _logger)
        {
            this._TPayPaymentSettings = TPayPaymentSettings;
            this._settingService = settingService;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._webHelper = webHelper;
            this._logger = _logger;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }


        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            ProcessPaymentResult result = new ProcessPaymentResult();
            CardsAPIApi api = new CardsAPIApi();
            //result.NewPaymentStatus = Core.Domain.Payments.PaymentStatus.Pending;
            return result;
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            string transactionDescription = GetTransactionDecription(postProcessPaymentRequest.Order);
            string hash = GenerateCheckSum(postProcessPaymentRequest);
            string url = GetTransactionUrl(this._TPayPaymentSettings.MerchantID,
                postProcessPaymentRequest.Order.OrderTotal,
                postProcessPaymentRequest.Order.Id.ToString(),
                transactionDescription,
                this._webHelper.GetStoreLocation(),
                hash,
                postProcessPaymentRequest.Order.BillingAddress.Email,
                postProcessPaymentRequest.Order.BillingAddress.FirstName + " " + postProcessPaymentRequest.Order.BillingAddress.LastName);
            this._logger.Information(url);
            HttpContext.Current.Response.Redirect(url);
            HttpContext.Current.Response.Flush();
        }

        private string GenerateCheckSum(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            return CreateMD5(String.Format("{0}{1}{2}{3}", 
                this._TPayPaymentSettings.MerchantID, 
                postProcessPaymentRequest.Order.OrderTotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), 
                postProcessPaymentRequest.Order.Id.ToString(), 
                this._TPayPaymentSettings.MerchantSecret));
        }

        private string GetTransactionDecription(Order order)
        {
            StringBuilder descriptionBuilder = new StringBuilder();
            foreach (var item in order.OrderItems)
            {
                descriptionBuilder.Append(item.Product.Name);
                descriptionBuilder.Append(" ");
            }

            if (!order.PickUpInStore && this._TPayPaymentSettings.IncludeShippingMethodInDescription)
            {
                descriptionBuilder.AppendFormat("+({0})", order.ShippingMethod);
            }

            return HttpContext.Current.Server.UrlEncode(descriptionBuilder.ToString());
        }

        private string GetTransactionUrl(int id, decimal total, string internalId, string description, string returnUrl, string hash, string email, string name)
        {
            return String.Format("https://secure.tpay.com?id={0}&md5sum={1}&crc={2}&kwota={3}&opis={4}&pow_url={5}&email={6}&nazwisko={7}&jezyk=PL", id,
                HttpContext.Current.Server.UrlEncode(hash),
                internalId,
                total.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
                description, HttpContext.Current.Server.UrlEncode(returnUrl),
                email,
                name);
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return this._TPayPaymentSettings.AdditionalFee;
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
            result.AddError("Refund not supportd");

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
                throw new ArgumentNullException("order");
            }
            return order.PaymentStatus == Core.Domain.Payments.PaymentStatus.Pending && (DateTime.UtcNow - order.CreatedOnUtc).TotalMinutes >= 1.0;
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
            TPayPaymentSettings settings = new TPayPaymentSettings
            {
                MerchantID = 0,
                MerchantSecret = String.Empty,
                IncludeShippingMethodInDescription = false,
                AdditionalFee = 0
            };

            this._settingService.SaveSetting<TPayPaymentSettings>(settings, 0);
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.Instructions", "");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.RedirectionTip", "You will be redirected to TPay site to complete the order.");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.MerchantID", "Merchant ID");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.MerchantId.Hint", "Enter Merchant ID provided by TPay.");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.MerchantSecret", "Merchant Secret");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.MerchantSecret.Hint", "Merchant Secret generated by you.");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.IncludeShippingMethodInDescription", "Include shipping method in transaction description");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.IncludeShippingMethodInDescription.Hint", "Include shipping method in transaction description");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.AdditionalFee", "Additional Fee");
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.Payments.TPay.AdditionalFee.Hint", "Enter Additional.");
            base.Install();
        }

        public override void Uninstall()
        {
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.Instructions");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.RedirectionTip");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.MerchantID");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.MerchantId.Hint");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.MerchantSecret");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.MerchantSecret.Hint");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.IncludeShippingMethodInDescription");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.IncludeShippingMethodInDescription.Hint");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.AdditionalFee");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.Payments.TPay.AdditionalFee.Hint");
            base.Uninstall();
        }
    }
}