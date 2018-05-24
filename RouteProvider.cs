using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Payments.Tpay
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority => 0;

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Payments.Tpay.Configure", "Plugins/PaymentTpay/Configure", new
            {
                controller = "TpayPayment",
                action = "Configure"
            }, new[]
            {
                "Nop.Plugin.Payments.Payu.Controllers"
            });
            routes.MapRoute("Plugin.Payments.Tpay.PaymentInfo", "Plugins/PaymentTpay/PaymentInfo", new
            {
                controller = "TpayPayment",
                action = "PaymentInfo"
            }, new[]
            {
                "Nop.Plugin.Payments.Tpay.Controllers"
            });
            routes.MapRoute("Plugin.Payments.Tpay.Return", "Plugins/PaymentTpay/Return", new
            {
                controller = "TpayPayment",
                action = "Return"
            }, new[]
            {
                "Nop.Plugin.Payments.Tpay.Controllers"
            });
        }
    }
}