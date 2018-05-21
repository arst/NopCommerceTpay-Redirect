﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.TPay.Models;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Payments.TPay.Controllers
{
    public class TpayPaymentController : BasePaymentController
    {
        private readonly ISettingService settingService;

        private readonly IPaymentService paymentService;

        private readonly IOrderService orderService;

        private readonly IOrderProcessingService orderProcessingService;

        private readonly TPayPaymentSettings TPayPaymentSettings;

        private readonly PaymentSettings paymentSettings;

        private readonly ILogger logger;

        public TpayPaymentController(ISettingService settingService, IPaymentService paymentService, IOrderService orderService, IOrderProcessingService orderProcessingService, TPayPaymentSettings PayuPaymentSettings, PaymentSettings paymentSettings, ILogger logger)
        {
            this.settingService = settingService;
            this.paymentService = paymentService;
            this.orderService = orderService;
            this.orderProcessingService = orderProcessingService;
            this.TPayPaymentSettings = PayuPaymentSettings;
            this.paymentSettings = paymentSettings;
            this.logger = logger;
        }

        [AdminAuthorize, ChildActionOnly]
        public ActionResult Configure()
        {
            ConfigurationViewModel model = new ConfigurationViewModel();
            model.AdditionalFee = this.TPayPaymentSettings.AdditionalFee;
            model.IncludeShippingMethodInDescription = this.TPayPaymentSettings.IncludeShippingMethodInDescription;
            model.MerchantId = this.TPayPaymentSettings.MerchantID;
            model.MerchantSecret = this.TPayPaymentSettings.MerchantSecret;
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
                this.TPayPaymentSettings.AdditionalFee = model.AdditionalFee;
                this.TPayPaymentSettings.IncludeShippingMethodInDescription = model.IncludeShippingMethodInDescription;
                this.TPayPaymentSettings.MerchantID = model.MerchantId;
                this.TPayPaymentSettings.MerchantSecret = model.MerchantSecret;
                this.settingService.SaveSetting<TPayPaymentSettings>(this.TPayPaymentSettings, 0);
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
        public async Task<string> Return(/*PayNotification notification*/)
        {
            /*
            TPayPaymentProcessor processor = this.paymentService.LoadPaymentMethodBySystemName("Payments.TPay") as TPayPaymentProcessor;
            if (processor == null || 
                !PaymentExtensions.IsPaymentMethodActive(processor, this.paymentSettings) || 
                !processor.PluginDescriptor.Installed)
            {
                throw new NopException("TPay payments module cannot be loaded");
            }
            int localOrderNumber = Convert.ToInt32(notification.TrCrc);
            Order order = this.orderService.GetOrderById(localOrderNumber);
            if (notification.TrStatus.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                if (this.orderProcessingService.CanMarkOrderAsPaid(order))
                {
                    order.AuthorizationTransactionId = notification.TranId;
                    this.orderService.UpdateOrder(order);
                    this.orderProcessingService.MarkOrderAsPaid(order);
                }
            }
            else
            {
                if (this.orderProcessingService.CanCancelOrder(order))
                {
                    order.AuthorizationTransactionId = notification.TranId;
                    order.AuthorizationTransactionResult = notification.TrError;
                    this.orderService.UpdateOrder(order);
                    this.orderProcessingService.CancelOrder(order, true);
                }
            }
            */
            return "TRUE";
        }
    }
}