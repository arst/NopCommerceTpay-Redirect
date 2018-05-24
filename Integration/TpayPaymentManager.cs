using System;
using System.Net.Http;
using System.Text;
using System.Web;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.Tpay.Infrastructure;
using Nop.Plugin.Payments.Tpay.Integration.Model;
using Nop.Plugin.Payments.TPay;
using RestSharp;

namespace Nop.Plugin.Payments.Tpay.Integration
{
    public class TpayPaymentManager : ITpayPaymentManager
    {
        private const string CreateTransactionOperationPath = "create"; 

        private readonly RestClient client;

        private readonly TpayPaymentSettings paymentSettings;

        private readonly IWebHelper webHelper;

        private readonly IWorkContext workContext;

        private readonly IStoreContext storeContext;

        public TpayPaymentManager(TpayPaymentSettings tPayPaymentSettings, IWebHelper webHelper, IWorkContext workContext, IStoreContext storeContext)
        {
            paymentSettings = tPayPaymentSettings;
            client = new RestClient($"https://secure.tpay.com/api/gw/{paymentSettings.ApiKey}/transaction");
            this.webHelper = webHelper;
            this.workContext = workContext;
            this.storeContext = storeContext;
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
                chargebackRequest = new TpayPartialChargebackRequest() 
                {
                    Amount = amount,
                    ApiPassword = paymentSettings.ApiPassword,
                    Title = capturedTransactionId
                };
            }
            else
            {
                chargebackRequest = new TpayChargebackRequest()
                {
                    ApiPassword = paymentSettings.ApiPassword,
                    Title = capturedTransactionId
                };
            }

            var request = GetPreconfiguredRequest(chargebackRequest);
            var response = client.ExecuteAsPost<TpayChargebackResponse>(request, HttpMethod.Post.ToString());
            
            return response.Data;
        }

        private RestRequest GetPreconfiguredRequest(object body)
        {
            RestRequest request = new RestRequest(CreateTransactionOperationPath, Method.POST)
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
            result.Id = paymentSettings.MerchantId;
            result.Email = order.BillingAddress.Email;
            result.Address = string.Concat(order.BillingAddress.Address1, " ", order.BillingAddress.Address2);
            result.Amount = order.OrderTotal;
            result.AcceptTos = (int)AcceptTos.Yes;
            result.ApiPassword = paymentSettings.ApiPassword;
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
            result.ResultEmail = paymentSettings.ResultEmail;
            var storeUri = new Uri(webHelper.GetStoreLocation());
            result.ResultUrl =
                "https://webhook.site/96e0a1aa-bf6c-438b-baf3-ce75cca9a78b";//new Uri(storeUri, "Plugins/PaymentTpay/Return").ToString();
            result.ReturnErrorUrl = paymentSettings.ReturnErrorUrl;
            result.ReturnUrl = paymentSettings.ReturnUrl;

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

            if (!order.PickUpInStore && paymentSettings.IncludeShippingMethodInDescription)
            {
                descriptionBuilder.AppendFormat("+({0})", order.ShippingMethod);
            }

            return HttpContext.Current.Server.UrlEncode(descriptionBuilder.ToString());
        }

        private string GenerateCheckSum(Order order)
        {
            return Md5HashManager.GetMd5Hash($"{paymentSettings.MerchantId}{order.OrderTotal:0.#####}{order.Id}{paymentSettings.MerchantSecret}");
        }
    }
}