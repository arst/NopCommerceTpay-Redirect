using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.Tpay.Integration.Model;
using Nop.Plugin.Payments.TPay.Controllers;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Payments;
using Nop.Services.Stores;
using RestSharp;

namespace Nop.Plugin.Payments.TPay
{
    public class TpayPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly IWorkContext workContext;

        private readonly TpayPaymentSettings tPayPaymentSettings;

        private readonly ISettingService settingService;

        private readonly IWebHelper webHelper;

        private readonly ILogger logger;

        private readonly ICustomerService customerService;

        private readonly IStoreContext storeContext;

        private readonly IStoreService storeService;

        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => true;

        public bool SupportRefund => true;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        public bool SkipPaymentInfo => false;

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart) => false;

        public string PaymentMethodDescription => "TPay";

        public TpayPaymentProcessor(TpayPaymentSettings tPayPaymentSettings, ISettingService settingService, ICurrencyService currencyService, CurrencySettings currencySettings, IWebHelper webHelper, ILogger logger, ICustomerService customerService, IStoreContext storeContext, IStoreService storeService, IWorkContext workContext)
        {
            this.tPayPaymentSettings = tPayPaymentSettings;
            this.settingService = settingService;
            this.webHelper = webHelper;
            this.logger = logger;
            this.customerService = customerService;
            this.storeContext = storeContext;
            this.storeService = storeService;
            this.workContext = workContext;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest) => new ProcessPaymentResult();

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var transaction = PrepareTransaction(postProcessPaymentRequest.Order);
            RestClient client = new RestClient($"https://secure.tpay.com/api/gw/{tPayPaymentSettings.ApiKey}/");
            RestRequest request = new RestRequest("transaction/create", Method.POST);
            request.AddBody(transaction);
            var response = client.Post<CreateTpayTransactionResponse>(request);

            if (!string.IsNullOrEmpty(response.Data.Err))
            {
                throw new NopException(response.Data.Err);
            }

            HttpContext.Current.Response.Redirect(response.Data.Url);
            HttpContext.Current.Response.Flush();
        }

        private TpayTransaction PrepareTransaction(Order order)
        {
            TpayTransaction result = new TpayTransaction();
            result.Email = order.BillingAddress.Email;
            result.Address = string.Concat(order.BillingAddress.Address1, " ", order.BillingAddress.Address2);
            result.Amount = order.OrderTotal;
            result.AcceptTos = (int)AcceptTos.Yes;
            result.ApiPassword = tPayPaymentSettings.ApiPassword;
            result.City = order.BillingAddress.City;
            result.Country = order.BillingAddress.Country.TwoLetterIsoCode;
            result.Crc = order.Id;
            result.CustomDescription = GetTransactionDecription(order);
            result.Description = $"Payment for order {order.Id}";
            result.Group = 150;
            result.Language = workContext.WorkingLanguage.Name;
            result.Md5Sum = GenerateCheckSum(order);
            result.Online = (int)Online.Yes;
            result.Zip = order.BillingAddress.ZipPostalCode;
            result.Name = string.Concat(order.BillingAddress.FirstName, " ", order.BillingAddress.LastName);
            result.MerchantDescription = storeContext.CurrentStore.Name;
            result.ResultEmail = tPayPaymentSettings.ResultEmail;
            var storeUri = new Uri(webHelper.GetStoreLocation());
            result.ResultUrl = new Uri(storeUri, "Plugins/PaymentTpay/Return").ToString();
            result.ReturnErrorUrl = tPayPaymentSettings.ReturnErrorUrl;
            result.ReturnUrl = tPayPaymentSettings.ReturnUrl;

            return result;
        }

        private string GetTransactionDecription(Order order)
        {
            StringBuilder descriptionBuilder = new StringBuilder();
            foreach (var item in order.OrderItems)
            {
                descriptionBuilder.Append(item.Product.Name);
                descriptionBuilder.Append(" ");
            }

            if (!order.PickUpInStore && tPayPaymentSettings.IncludeShippingMethodInDescription)
            {
                descriptionBuilder.AppendFormat("+({0})", order.ShippingMethod);
            }

            return HttpContext.Current.Server.UrlEncode(descriptionBuilder.ToString());
        }

        private string GenerateCheckSum(Order order)
        {
            return CreateMD5(
                $"{tPayPaymentSettings.MerchantID}{order.OrderTotal.ToString("0.00", CultureInfo.InvariantCulture)}{order.Id.ToString()}{tPayPaymentSettings.MerchantSecret}");
        }

        public static string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var hashedByte in hashBytes)
                {
                    sb.Append(hashedByte.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string
            GetTransactionUrl(int id, decimal total, string internalId, string description, string returnUrl,
                string hash, string email, string name)
        {
            return
                $"https://secure.tpay.com?id={id}&md5sum={HttpContext.Current.Server.UrlEncode(hash)}&crc={internalId}&kwota={total.ToString("0.00", CultureInfo.InvariantCulture)}&opis={description}&pow_url={HttpContext.Current.Server.UrlEncode(returnUrl)}&email={email}&nazwisko={name}&jezyk={tPayPaymentSettings.Language}";
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
                MerchantID = 0,
                MerchantSecret = String.Empty,
                IncludeShippingMethodInDescription = false,
                AdditionalFee = 0
            };

            settingService.SaveSetting(settings, 0);
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
            base.Uninstall();
        }
    }
}