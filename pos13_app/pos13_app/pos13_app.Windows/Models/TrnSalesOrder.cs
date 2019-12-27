using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos13_app.Models
{
    class TrnSalesOrder
    {
        public int Id { get; set; }
        public string SalesNumber { get; set; }
        public decimal Amount { get; set; }
        public string TableCode { get; set; }
        public string User { get; set; }
        public string InvoiceStatus { get; set; }
    }
}
