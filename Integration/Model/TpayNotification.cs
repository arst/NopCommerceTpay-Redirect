using System;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace Nop.Plugin.Payments.Tpay.Integration.Model
{
    public class TpayNotification
    {
        public int Id { get; set; }

        public string tr_id { get; set; }

        public DateTime tr_date { get; set; }

        public string tr_crc { get; set; }

        public string tr_amount { get; set; }

        public string tr_paid { get; set; }

        public string tr_desc { get; set; }

        public string tr_status { get; set; }

        public string tr_error { get; set; }

        public string tr_email { get; set; }

        public string Md5Sum { get; set; }

        public int test_mode { get; set; }

        public string Wallet { get; set; }
    }
}
