using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Payments.Payu
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            RouteCollectionExtensions.MapRoute(routes, "Plugin.Payments.Tpay.Configure", "Plugins/PaymentTpay/Configure", new
            {
                controller = "TpayPayment",
                action = "Configure"
            }, new string[]
            {
                "Nop.Plugin.Payments.Payu.Controllers"
            });
            RouteCollectionExtensions.MapRoute(routes, "Plugin.Payments.Tpay.PaymentInfo", "Plugins/PaymentTpay/PaymentInfo", new
            {
                controller = "TpayPayment",
                action = "PaymentInfo"
            }, new string[]
            {
                "Nop.Plugin.Payments.Tpay.Controllers"
            });
            RouteCollectionExtensions.MapRoute(routes, "Plugin.Payments.Tpay.Return", "Plugins/PaymentTpay/Return", new
            {
                controller = "TpayPayment",
                action = "Return"
            }, new string[]
            {
                "Nop.Plugin.Payments.Tpay.Controllers"
            });
        }
    }
}