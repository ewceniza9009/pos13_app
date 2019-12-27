using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using pos13_app_data.Controllers;
using pos13_app_data.Data;
using pos13_app_data.Models;

namespace pos13_app_data
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MstUserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MstUserService.svc or MstUserService.svc.cs at the Solution Explorer and start debugging.
    public class Pos13_app_service : IPos13_app_service
    {
        #region MstUser Service
        private MstUserController _mstUserController = new MstUserController();
        public string getUserName(int id)
        {
            return _mstUserController.getUserName(id);
        }
        public int getUserId(string username, string password)
        {
            return _mstUserController.getUserId(username, password);
        }
        public Boolean isLogin(string username, string password)
        {
            return _mstUserController.isLogin(username, password);
        }
        public List<MstUserController> UserCombo()
        {
            return _mstUserController.UserCombo();
        }
        #endregion

        #region MstTable Service
        private MstTableController mstTable = new MstTableController();
        public List<MstTableController> GetMstTable(int TableGroupId)
        {
            return mstTable.GetMstTable(TableGroupId);
        }
        #endregion

        #region MstTableGroup Service
        private MstTableGroupController mstTableGroup = new MstTableGroupController();
        public List<MstTableGroupController> GetMstTableGroup()
        {
            return mstTableGroup.GetMstTableGroup();
        }
        public int MinTableGroupId()
        {
            return mstTableGroup.MinTableGroupId();
        }
        #endregion

        #region TrnSalesOrder Service
        private TrnSalesOrderController _trnSalesOrderController = new TrnSalesOrderController();
        public List<TrnSalesOrderController> TrnSalesOrderListSF1(DateTime SalesDate,string InvoiceStatus)
        {
            return _trnSalesOrderController.TrnSalesOrderListSF1(SalesDate,InvoiceStatus);
        }
        public List<TrnSalesOrderController> TrnSalesOrderDetail(int InvoiceId)
        {
            return _trnSalesOrderController.TrnSalesOrderDetail(InvoiceId);
        }
        public List<TrnSalesOrderController> TrnSalesOrderDetailZero(int InvoiceId)
        {
            return _trnSalesOrderController.TrnSalesOrderDetailZero(InvoiceId);
        }
        public int GetTableInvoiceId(DateTime SalesDate, int TableId, string InvoiceStatus)
        {
            return _trnSalesOrderController.GetTableInvoiceId(SalesDate, TableId, InvoiceStatus);
        }

        #endregion

        #region TrnSalesOrderDetail Service
        private TrnSalesOrderDetailController _trnSalesOrderDetailController = new TrnSalesOrderDetailController();
        public List<TrnSalesOrderDetailController> TrnSalesOrderDetailItem(int InvoiceId)
        {
            return _trnSalesOrderDetailController.TrnSalesOrderDetalItem(InvoiceId);
        }

        public TrnSalesOrderDetailController TrnSalesTrnSalesOrderDetailItemDetail(int InvoiceLineId)
        {
            return _trnSalesOrderDetailController.TrnSalesOrderDetailItemDetail(InvoiceLineId);
        }
        #endregion

        #region MstItemGroup Service
        private MstItemGroupController _mstItemGroupController = new MstItemGroupController();
        public List<MstItemGroupController> GetMstItemGroup()
        {
            return _mstItemGroupController.GetMstItemGroup();
        }
        public int MinItemGroupId()
        {
            return _mstItemGroupController.MinItemGroupId();
        }
        #endregion

        #region MstItem Service 
        public MstItemController _mstItemController = new MstItemController();
        public List<MstItemController> GetMstItem(int ItemGroupId)
        {
            return _mstItemController.GetMstItem(ItemGroupId);
        }

        public List<Models.MstItem> ItemComboBox()
        {
            return _mstItemController.ItemComboBox();
        }
        #endregion

        #region MstUnit Service
        public MstUnitController _mstUnitController =  new MstUnitController();
        public List<MstUnitController> UnitCombo()
        {
            return _mstUnitController.UnitCombo();
        }
        #endregion

        #region MstTax Service
        public MstTaxController _mstTaxController = new MstTaxController();
        public List<MstTaxController> TaxCombo()
        {
            return _mstTaxController.TaxCombo();
        }

        #endregion

        #region MstCustomer Service
        public MstCustomerController _mstCustomerController = new MstCustomerController();
        public List<MstCustomerController> CustomerCombo()
        {
            return _mstCustomerController.CustomerCombo();
        }

        #endregion

        #region MstTerm Service
        public MstTermController _mstTermController = new MstTermController();
        public List<MstTermController> TermCombo()
        {
            return _mstTermController.TermCombo();
        }

        #endregion

        #region MstDiscount Service
        public MstDiscountController _mstDiscountController = new MstDiscountController();
        public List<MstDiscountController> DiscountCombo(DateTime SalesDate,int ItemId)
        {
            return _mstDiscountController.DiscountCombo(SalesDate,ItemId);
        }
        #endregion

        #region TrnSale Service
        public void UpdateInvoiceTotalAmount(int InvoiceId)
        {
            _trnSalesOrderController.UpdateInvoiceTotalAmount(InvoiceId);
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
            _trnSalesOrderController.InserSalesOrder(
                Id,
                PeriodId,
                SalesDate,
                SalesNumber,
                ManualInvoiceNumber,
                Amount,
                TableId,
                CustomerId,
                AccountId,
                TermId,
                DiscountId,
                SeniorCitizenId,
                SeniorCitizenName,
                SeniorCitizenAge,
                Remarks,
                SalesAgent,
                TerminalId,
                PreparedBy,
                CheckedBy,
                ApprovedBy,
                IsLocked,
                IsCancelled,
                PaidAmount,
                CreditAmount,
                DebitAmount,
                BalanceAmount,
                EntryUserId,
                EntryDateTime,
                UpdateUserId,
                UpdateDateTime,
                Pax,
                Exec
                );
        }
        public void DeleteSales(int SalesId)
        {
            _trnSalesOrderController.DeleteSales(SalesId);
        }
        #endregion

        #region TrnSalesLine Service 
        public void InsertSalesOrderLine(
            int SalesLineId,
            int ItemId,
            int SalesId,
            int UnitId,
            decimal Price,
            int DiscountId,
            decimal DiscountRate,
            decimal DiscountAmount,
            decimal NetPrice,
            decimal Quantity,
            decimal Amount,
            int TaxId,
            decimal TaxRate,
            decimal TaxAmount,
            int SalesAccountId,
            int AssetAccountId,
            int CostAccountId,
            int TaxAccountId,
            DateTime SalesLineTimeStamp,
            int UserId,
            string Preparation
            )
        {
            _trnSalesOrderDetailController.InserSalesOrderLine(
                SalesLineId,
                SalesId,
                ItemId,
                UnitId,
                Price,
                DiscountId,
                DiscountRate,
                DiscountAmount,
                NetPrice,
                Quantity,
                Amount,
                TaxId,
                TaxRate,
                TaxAmount,
                SalesAccountId,
                AssetAccountId,
                CostAccountId,
                TaxAccountId,
                SalesLineTimeStamp,
                UserId,
                Preparation
                );
        }
        public void DeleteSalesLine(int SalesLineId)
        {
            _trnSalesOrderDetailController.DeleteSalesLine(SalesLineId);
        }
        #endregion
    }
}
