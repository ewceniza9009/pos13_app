using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using pos13_app_data.Controllers;

namespace pos13_app_data
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMstUserService" in both code and config file together.
    [ServiceContract]
    public interface IPos13_app_service
    {
        #region MstUser
        [OperationContract]
        string getUserName(int id);

        [OperationContract]
        int getUserId(string username, string password);

        [OperationContract]
        Boolean isLogin(string username, string password);

        [OperationContract]
        List<MstUserController> UserCombo();
        #endregion

        #region MstTable
        [OperationContract]
        List<MstTableController> GetMstTable(int TableGroupId);
        #endregion

        #region MstTableGroup
        [OperationContract]
        List<MstTableGroupController> GetMstTableGroup();
        [OperationContract]
        int MinTableGroupId();
        #endregion

        #region TrnSalesOrder
        [OperationContract]
        List<TrnSalesOrderController> TrnSalesOrderListSF1(DateTime SalesDate,string InvoiceStatus);
        [OperationContract]
        List<TrnSalesOrderController> TrnSalesOrderDetail(int InvoiceId);
        [OperationContract]
        List<TrnSalesOrderController> TrnSalesOrderDetailZero(int InvoiceId);
        [OperationContract]
        int GetTableInvoiceId(DateTime SalesDate, int TableId,string InvoiceStatus);
        #endregion

        #region TrnSalesOrderDetail
        [OperationContract]
        List<TrnSalesOrderDetailController> TrnSalesOrderDetailItem(int SalesId);
        [OperationContract]
        TrnSalesOrderDetailController TrnSalesTrnSalesOrderDetailItemDetail(int InvoiceLineId);
        #endregion

        #region MstItemGroup
        [OperationContract]
        List<MstItemGroupController> GetMstItemGroup();
        [OperationContract]
        int MinItemGroupId();
        #endregion

        #region MstItem
        [OperationContract]
        List<MstItemController> GetMstItem(int ItemGroupId);

        [OperationContract]
        List<Models.MstItem> ItemComboBox();
        #endregion

        #region MstUnit
        [OperationContract]
        List<MstUnitController> UnitCombo();
        #endregion

        #region MstTax
        [OperationContract]
        List<MstTaxController> TaxCombo();
        #endregion

        #region MstDiscount
        [OperationContract]
        List<MstDiscountController> DiscountCombo(DateTime SalesDate,int ItemId);
        #endregion

        #region MstCustomer
        [OperationContract]
        List<MstCustomerController> CustomerCombo();
        #endregion

        #region MstTerm
        [OperationContract]
        List<MstTermController> TermCombo();
        #endregion

        #region TrnSale
        [OperationContract]
        void InserSalesOrder(
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
            );

        [OperationContract]
        void UpdateInvoiceTotalAmount(int InvoiceId);

        [OperationContract]
        void DeleteSales(int SalesId);
        #endregion

        #region TrnSalesLine
        [OperationContract]
        void InsertSalesOrderLine(
            int SalesLineId,
            int SalesId,
            int ItemId,
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
            );
        [OperationContract]
        void DeleteSalesLine(int SalesLineId);
        #endregion
    }
}
