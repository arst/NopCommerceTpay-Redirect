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
        public class TpayPaymentManager {
        private const string createTransactionOperationPath = "create"; 

        private readonly RestClient client;
        private readonly TpayPaymentSettings paymentSettings;
        private readonly IWebHelper webHelper;
        private readonly IWorkContext workContext;

        public TpayPaymentManager(TpayPaymentSettings tPayPaymentSettings, IWebHelper webHelper, IWorkContext workContext)
        {
            paymentSettings = tPayPaymentSettings;
            client = new RestClient($"https://secure.tpay.com/api/gw/{paymentSettings.ApiKey}/transaction");
            webHelper = webHelper;
            workContext = workContext;
        }

        public CreateTpayTransactionResponse CreatePayment(Order order)
        {
            var request = GetPreconfiguredRequest(ResolveTransaction(order));
            var response = client.Post<CreateTpayTransactionResponse>(request);

            return response.Data;
        }

        public TpayChargebackResponse CreateChargeback(string capturedTransactionId, decimal amount, bool isPartial)
        {
            TpayChargebackRequest chargebackRequest;

            if (isPartial)
            {
                var chargebackRequest = new TpayPartialChargebackRequest() 
                {
                    Amount = amount,
                    ApiPassword = paymentSettings.ApiPassword,
                    Title = capturedTransactionId;
                };
            }
            else
            {
                chargebackRequest = new TpayChargebackRequest()
                {
                    ApiPassword = tPayPaymentSettings.ApiPassword,
                    Title = capturedTransactionId
                };
            }

            var request = GetPreconfiguredRequest(chargebackRequest);
            var response = client.ExecuteAsPost<TpayChargebackResponse>(request, HttpMethod.Post.ToString());
            
            return response.Data;
        }

        private RestRequest GetPreconfiguredRequest(object body)
        {
            RestRequest request = new RestRequest(createTransactionOperationPath, Method.POST)
            {
                JsonSerializer = new RestSharpJsonNetSerializer()
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json; charset=utf-8", request.JsonSerializer.Serialize(body), ParameterType.RequestBody);

            return request;
        }

        private TpayTransaction ResolveTransaction(Order order)
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
            return MD5HashManager.GetMd5Hash($"{paymentSettings.MerchantId}{order.OrderTotal:0.#####}{order.Id}{paymentSettings.MerchantSecret}");
        }
    }
}