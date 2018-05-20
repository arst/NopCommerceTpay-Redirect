using System;

namespace Nop.Plugin.Payments.TPay.Integration.Model
{
    public class TPayNotification
    {
        public int Id { get; set; }
        public string TranId { get; set; }
        public DateTime TrDate { get; set; }
        public string TrCrc { get; set; }
        public string TrPaid { get; set; }
        public string TrDesc { get; set; }
        public string TrStatus { get; set; }
        public string TrError { get; set; }
        public string TrEmail { get; set; }
        public string Md5Sum { get; set; }
        public int TestMode { get; set; }
        public string Wallet { get; set; }
    }
}
