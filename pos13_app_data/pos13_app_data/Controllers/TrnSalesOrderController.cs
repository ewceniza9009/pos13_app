using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class TrnSalesOrderController
    {
        public int Id { get; set; }
        public DateTime SalesDate { get; set; }      
        public string SalesNumber { get; set; }
        public decimal Amount { get; set; }
        public string AmountDescription { get; set; }
        public string TableCode { get; set; }
        public string InvoiceTableId { get; set; }
        public string User { get; set; }
        public int CustomerId { get; set; }
        public int TermId { get; set; }
        public int SalesAgent{ get; set; }
        public string Remarks { get; set; }
        public int? Pax { get; set; }
        public string InvoiceStatus { get; set; }

        //For Sales Order Detail with param InvoiceId
        public string TransactionDate { get; set; }
        public string Terminal { get; set; }
        public string PreparedBy { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal LessAmount { get; set; }
        public string GrossAmountDescription { get; set; }
        public string LessAmountDescription { get; set; }

        public List<TrnSalesOrderController> TrnSalesOrderListSF1(DateTime SalesDate,string InvoiceStatus)
        {
         
            var data = new pos13_app_dataDataContext();

            var trnSalesOrderQ002 = from ss in data.TrnSales
                join tb in data.MstTables on ss.TableId equals tb.Id
                join us in data.MstUsers on ss.PreparedBy equals us.Id
                where ss.SalesDate == SalesDate && ss.TableId != null
                select new TrnSalesOrderController()
                {
                    Id = ss.Id,
                    SalesNumber = ss.SalesNumber,
                    Amount = ss.Amount,
                    TableCode = tb.TableCode,
                    User = us.FullName,
                    InvoiceTableId = ss.Id + "-" + ss.TableId,
                    InvoiceStatus =
                        ss.IsLocked == false && ss.IsCancelled == false
                            ? "Open"
                            : ss.IsLocked == true && ss.IsCancelled == false ? "Billed" : "Collected"
                };

            var trnSalerOrderQ004 = from i in trnSalesOrderQ002.AsEnumerable()
                where i.InvoiceStatus == InvoiceStatus
                select new
                {
                    Id = i.Id,
                    SalesNumber = i.SalesNumber,
                    Amount = String.Format("{0:c}", i.Amount),
                    TableCode = i.TableCode,
                    User = i.User,
                    InvoiceTableId = i.InvoiceTableId,
                    InvoiceStatus = i.InvoiceStatus
                };

            var trnSalerOrderQ006 = from i in trnSalerOrderQ004
                select new TrnSalesOrderController()
                {
                    Id = i.Id,
                    SalesNumber = i.SalesNumber,
                    AmountDescription = String.Format("{0:c}", i.Amount),
                    TableCode = i.TableCode,
                    User = i.User,
                    InvoiceTableId = i.InvoiceTableId,
                    InvoiceStatus = i.InvoiceStatus
                };

            return trnSalerOrderQ006.ToList();
        }

        public List<TrnSalesOrderController> TrnSalesOrderDetail(int InvoiceId)
        {
            var data = new pos13_app_dataDataContext();

            var trnSalesOrderQ002 = from sl in data.TrnSalesLines
                join ss in data.TrnSales on sl.SalesId equals ss.Id
                join tb in data.MstTables on ss.TableId equals tb.Id
                join us in data.MstUsers on ss.PreparedBy equals us.Id
                join td in data.MstTerminals on ss.TerminalId equals td.Id
                where ss.Id == InvoiceId
                group sl by new
                {
                    ss.Id,
                    ss.SalesDate,
                    ss.SalesNumber,
                    ss.CustomerId,
                    ss.TermId,
                    ss.SalesAgent,
                    td.Terminal,
                    tb.TableCode,
                    ss.Remarks,
                    ss.Pax,
                    us.FullName,
                    ss.Amount,
                    InvoiceStatus =
                        ss.IsLocked == false && ss.IsCancelled == false
                        ? "Open"
                        : ss.IsLocked == true && ss.IsCancelled == false ? "Billed" : "Collected"
                }
                into so
                select new
                {
                    Id = so.Key.Id,
                    TransactionDate = so.Key.SalesDate,
                    SalesDate = so.Key.SalesDate,
                    SalesNumber = so.Key.SalesNumber,
                    CustomerId = so.Key.CustomerId,
                    TermId = so.Key.TermId,
                    SalesAgent = so.Key.SalesAgent,
                    Remarks = so.Key.Remarks,
                    Pax = so.Key.Pax,
                    TerminalDescription = so.Key.Terminal,
                    TableDescription = so.Key.TableCode,
                    PreparedBy = so.Key.FullName,
                    GrossAmount = so.Sum(i => i.Price * i.Quantity),
                    LessAmount = so.Sum(i => i.Price * i.Quantity) - so.Key.Amount,
                    Amount = so.Key.Amount,
                    InvoiceStatus = so.Key.InvoiceStatus
                };

            var trnSalesOrderQ004 = from i in trnSalesOrderQ002.AsEnumerable()
                select new
                {
                    Id = i.Id,
                    SalesDate = i.SalesDate,
                    TransactionDate = "Transaction Date: " + String.Format("{0:d}",i.SalesDate),
                    SalesNumber = "Invoice No: " + i.SalesNumber,
                    Terminal = "Terminal: " + i.TerminalDescription + " Table: " + i.TableDescription,
                    PreparedBy = "Prepared By: " + i.PreparedBy,
                    GrossAmount = String.Format("{0:c}", i.GrossAmount),
                    LessAmount = String.Format("{0:c}", i.LessAmount),
                    Amount = String.Format("{0:c}", i.Amount),
                    CustomerId = i.CustomerId,
                    TermId = i.TermId,
                    SalesAgent = i.SalesAgent,
                    Remarks = i.Remarks,
                    Pax = i.Pax,
                    InvoiceStatus = i.InvoiceStatus
                };

            var trnSalesOrderQ006 = from i in trnSalesOrderQ004
                select new TrnSalesOrderController()
                {
                    Id = i.Id,
                    SalesDate = i.SalesDate,
                    TransactionDate = i.TransactionDate,
                    SalesNumber = i.SalesNumber,
                    Terminal = i.Terminal,
                    PreparedBy = i.PreparedBy,
                    GrossAmountDescription = i.GrossAmount,
                    LessAmountDescription = i.LessAmount,
                    AmountDescription = i.Amount,
                    CustomerId = i.CustomerId,
                    TermId = i.TermId,
                    SalesAgent = i.SalesAgent,
                    Remarks = i.Remarks,
                    Pax = i.Pax,
                    InvoiceStatus = i.InvoiceStatus
                };

            return trnSalesOrderQ006.ToList();

        }
        public List<TrnSalesOrderController> TrnSalesOrderDetailZero(int InvoiceId)
        {
            var data = new pos13_app_dataDataContext();

            var trnSalesOrderQ002 = from ss in data.TrnSales
                                    join tb in data.MstTables on ss.TableId equals tb.Id
                                    join us in data.MstUsers on ss.PreparedBy equals us.Id
                                    join td in data.MstTerminals on ss.TerminalId equals td.Id
                                    where ss.Id == InvoiceId
                                    select new TrnSalesOrderController()
                                    {
                                        Id = ss.Id,
                                        SalesDate = ss.SalesDate,
                                        TransactionDate = "Transaction Date: " + ss.SalesDate,
                                        SalesNumber = "Invoice No: " + ss.SalesNumber,
                                        Terminal = "Terminal: " + td.Terminal + " Table: " + tb.TableCode,
                                        PreparedBy = "Prepared By: " + us.FullName,
                                        GrossAmountDescription = "₱0.00",
                                        LessAmountDescription = "₱0.00",
                                        AmountDescription = "₱0.00",
                                        CustomerId = ss.CustomerId,
                                        TermId = ss.TermId,
                                        SalesAgent = ss.SalesAgent,
                                        Remarks = ss.Remarks,
                                        Pax = ss.Pax,
                                        InvoiceStatus = "Open"
                                    };
            return trnSalesOrderQ002.ToList();

        }

        public int GetTableInvoiceId(DateTime SalesDate, int TableId,string InvoiceStatus)
        {
            var data = new pos13_app_dataDataContext();

            var trnSalesQ002 = from i in data.TrnSales
                where i.SalesDate == SalesDate && i.TableId == TableId
                select new
                {
                    InvoiceId = i.Id,
                    InvoiceStat = i.IsLocked == false && i.IsCancelled == false
                    ? "Open"
                    : i.IsLocked == true && i.IsCancelled == false ? "Billed" : "Collected"
                };

            var InvoiceId = (from i in trnSalesQ002
                where  i.InvoiceStat == InvoiceStatus
                select i.InvoiceId).FirstOrDefault();

            return InvoiceId;
        }
        public void InserSalesOrder(
            int Id,
            int PeriodId,
            DateTime SalesDate,
            string SalesNumber,
            string ManualInvoiceNumber,
            decimal Amount,
            int TableId,
            int CustomerId,
            int AccountId,
            int TermId,
            int DiscountId,
            string SeniorCitizenId,
            string SeniorCitizenName,
            int SeniorCitizenAge,
            string Remarks,
            int SalesAgent,
            int TerminalId,
            int PreparedBy,
            int CheckedBy,
            int ApprovedBy,
            bool IsLocked,
            bool IsCancelled,
            decimal PaidAmount,
            decimal CreditAmount,
            decimal DebitAmount,
            decimal BalanceAmount,
            int EntryUserId,
            DateTime EntryDateTime,
            int UpdateUserId,
            DateTime UpdateDateTime,
            int Pax,
            string Exec
            )
        {
            var data = new pos13_app_dataDataContext();

            var MaxSalesNumber = data.TrnSales.Max(i => i.SalesNumber) ?? "0001-000001";

            var StringMaxNumber = (int.Parse(MaxSalesNumber.Substring(MaxSalesNumber.Length - 6, 6)) + 1000001).ToString();
            var StringNewNumber = StringMaxNumber.Substring(StringMaxNumber.Length - 6, 6);
            var Period = MaxSalesNumber.Substring(0, 4);

            var NewSalesNumber = Period + "-" + StringNewNumber;

            SalesNumber = NewSalesNumber;
            ManualInvoiceNumber = NewSalesNumber;

            var Sales = new TrnSales()
            {
                Id = Id,
                PeriodId = PeriodId,
                SalesDate = SalesDate,
                SalesNumber = SalesNumber,
                ManualInvoiceNumber = ManualInvoiceNumber,
                Amount = Amount,
                TableId = TableId,
                CustomerId = CustomerId,
                AccountId = AccountId,
                TermId = TermId,
                DiscountId = DiscountId,
                SeniorCitizenId = SeniorCitizenId,
                SeniorCitizenName = SeniorCitizenName,
                SeniorCitizenAge = SeniorCitizenAge,
                Remarks = Remarks,
                SalesAgent = SalesAgent,
                TerminalId = TerminalId,
                PreparedBy = PreparedBy,
                CheckedBy = CheckedBy,
                ApprovedBy = ApprovedBy,
                IsLocked = IsLocked,
                IsCancelled = IsCancelled,
                PaidAmount = PaidAmount,
                CreditAmount = CreditAmount,
                DebitAmount = DebitAmount,
                BalanceAmount = BalanceAmount,
                EntryUserId = EntryUserId,
                EntryDateTime = EntryDateTime,
                UpdateUserId = UpdateUserId,
                UpdateDateTime = UpdateDateTime,
                Pax = Pax,
            };

            if (Id > 0)
            {
                var trnsale = data.TrnSales.Single(i => i.Id == Id);

                trnsale.PeriodId = Sales.PeriodId;
                trnsale.SalesDate = Sales.SalesDate;
                trnsale.SalesNumber = Sales.SalesNumber;
                trnsale.ManualInvoiceNumber = Sales.ManualInvoiceNumber;
                trnsale.Amount = Sales.Amount;
                trnsale.TableId = Sales.TableId;
                trnsale.CustomerId = Sales.CustomerId;
                trnsale.AccountId = Sales.AccountId;
                trnsale.TermId = Sales.TermId;
                trnsale.DiscountId = Sales.DiscountId;
                trnsale.SeniorCitizenId = Sales.SeniorCitizenId;
                trnsale.SeniorCitizenName = Sales.SeniorCitizenName;
                trnsale.SeniorCitizenAge = Sales.SeniorCitizenAge;
                trnsale.Remarks = Sales.Remarks;
                trnsale.SalesAgent = Sales.SalesAgent;
                trnsale.TerminalId = Sales.TerminalId;
                trnsale.PreparedBy = Sales.PreparedBy;
                trnsale.CheckedBy = Sales.CheckedBy;
                trnsale.ApprovedBy = Sales.ApprovedBy;
                trnsale.IsLocked = Sales.IsLocked;
                trnsale.IsCancelled = Sales.IsCancelled;
                trnsale.PaidAmount = Sales.PaidAmount;
                trnsale.CreditAmount = Sales.CreditAmount;
                trnsale.DebitAmount = Sales.DebitAmount;
                trnsale.BalanceAmount = Sales.BalanceAmount;
                trnsale.EntryUserId = Sales.EntryUserId;
                trnsale.EntryDateTime = Sales.EntryDateTime;
                trnsale.UpdateUserId = Sales.UpdateUserId;
                trnsale.UpdateDateTime = Sales.UpdateDateTime;
                trnsale.Pax = Sales.Pax;

                data.SubmitChanges();
            }
            else 
            {
                var trnsale = new TrnSale()
                {
                    PeriodId = Sales.PeriodId,
                    SalesDate = Sales.SalesDate,
                    SalesNumber = Sales.SalesNumber,
                    ManualInvoiceNumber = Sales.ManualInvoiceNumber,
                    Amount = Sales.Amount,
                    TableId = Sales.TableId,
                    CustomerId = Sales.CustomerId,
                    AccountId = Sales.AccountId,
                    TermId = Sales.TermId,
                    DiscountId = Sales.DiscountId,
                    SeniorCitizenId = Sales.SeniorCitizenId,
                    SeniorCitizenName = Sales.SeniorCitizenName,
                    SeniorCitizenAge = Sales.SeniorCitizenAge,
                    Remarks = Sales.Remarks,
                    SalesAgent = Sales.SalesAgent,
                    TerminalId = Sales.TerminalId,
                    PreparedBy = Sales.PreparedBy,
                    CheckedBy = Sales.CheckedBy,
                    ApprovedBy = Sales.ApprovedBy,
                    IsLocked = Sales.IsLocked,
                    IsCancelled = Sales.IsCancelled,
                    PaidAmount = Sales.PaidAmount,
                    CreditAmount = Sales.CreditAmount,
                    DebitAmount = Sales.DebitAmount,
                    BalanceAmount = Sales.BalanceAmount,
                    EntryUserId = Sales.EntryUserId,
                    EntryDateTime = Sales.EntryDateTime,
                    UpdateUserId = Sales.UpdateUserId,
                    UpdateDateTime = Sales.UpdateDateTime,
                    Pax = Sales.Pax
                };

                data.TrnSales.InsertOnSubmit(trnsale);
                data.SubmitChanges();
            }
        }

        public void UpdateInvoiceTotalAmount(int InvoiceId)
        {
            var data = new pos13_app_dataDataContext();
            var trnsale = data.TrnSales.Single(i => i.Id == InvoiceId);

            var TotalAmount = (from i in data.TrnSalesLines
                where i.SalesId == InvoiceId
                select i.Amount).Sum();

            trnsale.Amount = TotalAmount;
            trnsale.BalanceAmount = TotalAmount;

            data.SubmitChanges();
        }

        public void DeleteSales(int SalesId)
        {
            var data = new pos13_app_dataDataContext();
            var trnsales = data.TrnSales.Single(i => i.Id == SalesId);

            data.TrnSales.DeleteOnSubmit(trnsales);
            data.SubmitChanges();
        }
    }
}