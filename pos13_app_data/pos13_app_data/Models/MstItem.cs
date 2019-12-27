using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pos13_app_data.Models
{
    public class MstItem
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public decimal Price { get; set; }
        public int OutTaxId { get; set; }
        public decimal VatRate { get; set; }
    }
}