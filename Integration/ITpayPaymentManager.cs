using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.Tpay.Integration.Model;

namespace Nop.Plugin.Payments.Tpay.Integration
{
    public interface ITpayPaymentManager
    {
        TpayChargebackResponse CreateChargeback(string capturedTransactionId, decimal amount, bool isPartial);
        CreateTpayTransactionResponse CreatePayment(Order order);
    }
}