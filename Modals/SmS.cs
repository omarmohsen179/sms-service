using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Sender.Modals
{
    public class SmS
    {
        [Key]
        public int Id { get; set; }

        public string MobileNum { get; set; }

        public string MessageText { get; set; }

        public string Finished { get; set; }

        public string NAME_CUST { get; set; }

        public string SentCode { get; set; }

        public string DeviceSerial { get; set; }

        public DateTime? READ_DATE2 { get; set; }

        public DateTime? READ_DATE1 { get; set; }

        public string Done { get; set; }

        public int? read_id { get; set; }

        public int? FromCustId { get; set; }

        public int? ToCustId { get; set; }

    }
}
