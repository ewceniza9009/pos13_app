using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pos13_app_data.Controllers
{
    public class TrnSales
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public DateTime SalesDate { get; set; }
        public string SalesNumber { get; set; }
        public string ManualInvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int TermId { get; set; }
        public int DiscountId { get; set; }
        public string SeniorCitizenId { get; set; }
        public string SeniorCitizenName { get; set; }
        public int SeniorCitizenAge { get; set; }
        public string Remarks { get; set; }
        public int SalesAgent { get; set; }
        public int TerminalId { get; set; }
        public int PreparedBy { get; set; }
        public int CheckedBy { get; set; }
        public int ApprovedBy { get; set; }
        public bool IsLocked { get; set; }
        public bool IsCancelled { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public int EntryUserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public int Pax { get; set; }

    }
}