using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendGrid
{
    public class Orders
    {
        public string B2BEECOMMERCEEMAIL { get; set; }
        public string AccountNumber { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ItemTitle { get; set; }
        public string ItemSku { get; set; }
        public int? Quantity { get; set; }
        public string Currency { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? SubTotal { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressCity { get; set; }
        public string StateCode { get; set; }
        public string AddressZip { get; set; }
    }
}
