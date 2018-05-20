using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.TPay.Model
{
    public class TPayNotification
    {
        public int id { get; set; }
        public string tran_id { get; set; }
        public DateTime tr_date { get; set; }
        public string tr_crc { get; set; }
        public string tr_paid { get; set; }
        public string tr_desc { get; set; }
        public string tr_status { get; set; }
        public string tr_error { get; set; }
        public string tr_email { get; set; }
        public string md5sum { get; set; }
        public int test_mode { get; set; }
        public string wallet { get; set; }
    }
}
