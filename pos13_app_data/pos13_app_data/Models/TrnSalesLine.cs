using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pos13_app_data.Models
{
    public class TrnSalesLine
    {
        public int Id { get; set; }
        public int SalesId { get; set; }
        public int ItemId { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public int DiscountId { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public int TaxId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public int SalesAccountId { get; set; }
        public int AssetAccountId { get; set; }
        public int CostAccountId { get; set; }
        public int TaxAccountId { get; set; }
        public DateTime SalesLineTimeStamp { get; set; }
        public int UserId { get; set; }
        public string Preparation { get; set; }
    }
}