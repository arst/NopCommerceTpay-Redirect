using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.Tpay.Integration.Model;
using Nop.Plugin.Payments.TPay.Controllers;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Routing;
using Nop.Plugin.Payments.Tpay.Infrastructure;

namespace Nop.Plugin.Payments.TPay
{
    public class TpayPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly IWorkContext workContext;

        private readonly TpayPaymentSettings tPayPaymentSettings;

        private readonly ISettingService settingService;

        private readonly IWebHelper webHelper;

        private readonly IStoreContext storeContext;

        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => true;

        public bool SupportRefund => true;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        public bool SkipPaymentInfo => false;

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart) => false;

        public string PaymentMethodDescription => "TPay";

        public TpayPaymentProcessor(TpayPaymentSettings tPayPaymentSettings, ISettingService settingService, IWebHelper webHelper, IStoreContext storeContext, IWorkContext workContext)
        {
            this.tPayPaymentSettings = tPayPaymentSettings;
            this.settingService = settingService;
            this.webHelper = webHelper;
            this.storeContext = storeContext;
            this.workContext = workContext;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest) => new ProcessPaymentResult();

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var transaction = PrepareTransaction(postProcessPaymentRequest.Order);
            RestClient client = new RestClient($"https://secure.tpay.com/api/gw/{tPayPaymentSettings.ApiKey}/");
            RestRequest request = new RestRequest("transaction/create", Method.POST)
            {
                JsonSerializer = new RestSharpJsonNetSerializer()
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json; charset=utf-8", request.JsonSerializer.Serialize(transaction), ParameterType.RequestBody);
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
            result.Id = tPayPaymentSettings.MerchantId;
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
            return CreateMd5(
                $"{tPayPaymentSettings.MerchantId}{order.OrderTotal:0.#####}{order.Id}{tPayPaymentSettings.MerchantSecret}");
        }

        public static string CreateMd5(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var hashedByte in hashBytes)
                {
                    sb.Append(hashedByte.ToString("x2"));
                }
                return sb.ToString();
            }
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
            RestClient client = new RestClient($"https://secure.tpay.com/api/gw/{tPayPaymentSettings.ApiKey}/chargeback/transaction");
            TpayChargebackRequest chargebackRequest;

            if (refundPaymentRequest.IsPartialRefund)
            {
                var partialRefundRequest = new TpayPartialChargebackRequest();
                partialRefundRequest.Amount = refundPaymentRequest.AmountToRefund;
                partialRefundRequest.ApiPassword = tPayPaymentSettings.ApiPassword;
                partialRefundRequest.Title = refundPaymentRequest.Order.CaptureTransactionId;
                chargebackRequest = partialRefundRequest;
            }
            else
            {
                chargebackRequest = new TpayChargebackRequest();
                chargebackRequest.ApiPassword = tPayPaymentSettings.ApiPassword;
                chargebackRequest.Title = refundPaymentRequest.Order.CaptureTransactionId;
            }

            var request = new RestRequest();
            request.AddBody(chargebackRequest);
            var response = client.ExecuteAsPost<TpayChargebackResponse>(request, HttpMethod.Post.ToString());

            if (response.Data.Result != TpayChargebackResult.Correct)
            {
                result.Errors.Add(response.Data.Error);
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
            base.Uninstall();
        }
    }
}