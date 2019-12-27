using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class TrnSalesOrderDetailController
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string SalesItemId { get; set; }
        public string ItemDescription { get; set; }
        public string QuantityDescription { get; set; }
        public string PriceDescription { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Tax { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public string AmountDescription { get; set; }

        public int UnitId { get; set; }
        public int DiscountId { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal NetPrice { get; set; }
        public int TaxId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }

        public List<TrnSalesOrderDetailController> TrnSalesOrderDetalItem(int SalesId)
        {
            var data = new pos13_app_dataDataContext();

            var trnSalesOrderDetailQ002 = from sl in data.TrnSalesLines
                join im in data.MstItems on sl.ItemId equals im.Id
                join ut in data.MstUnits on sl.UnitId equals ut.Id
                join tx in data.MstTaxes on sl.TaxId equals tx.Id
                join ur in data.MstUsers on sl.UserId equals ur.Id
                where sl.SalesId == SalesId
                select new
                {
                    Id = sl.Id,
                    ItemId = sl.ItemId,
                    ItemDescription = im.ItemDescription,
                    Quantity = sl.Quantity,
                    Unit = ut.Unit,
                    Price = sl.Price,
                    DiscountAmount = sl.DiscountAmount,
                    Tax = tx.Tax,
                    UserName = ur.UserName,
                    PriceDescription = "NA",
                    Amount = sl.Amount
                };
            var trnSalesOrderDetailQ004 = from i in trnSalesOrderDetailQ002.AsEnumerable()
                let priceDescription =
                    i.Unit + " @ " + String.Format("{0:c}",i.Price) + " Less: " + String.Format("{0:c}",i.DiscountAmount) + " - " + i.Tax + " - " + i.UserName
                select new
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    SalesItemId = i.Id + "-" + i.ItemId,
                    ItemDescription = i.ItemDescription,
                    QuantityDescription = String.Format("{0:N}",i.Quantity),
                    Unit = i.Unit,
                    Price = i.Price,
                    DiscountAmount = i.DiscountAmount,
                    Tax = i.Tax,
                    UserName = i.UserName,
                    PriceDescription = priceDescription,
                    AmountDescription = String.Format("{0:c}",i.Amount)
                };

            var trnSalesOrderDetailQ006 = from i in trnSalesOrderDetailQ004
                select new TrnSalesOrderDetailController()
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    SalesItemId = i.SalesItemId,
                    ItemDescription = i.ItemDescription,
                    QuantityDescription = i.QuantityDescription,
                    Unit = i.Unit,
                    Price = i.Price,
                    DiscountAmount = i.DiscountAmount,
                    Tax = i.Tax,
                    UserName = i.UserName,
                    PriceDescription = i.PriceDescription,
                    AmountDescription = i.AmountDescription
                };
            return trnSalesOrderDetailQ006.ToList();
        }
        public TrnSalesOrderDetailController TrnSalesOrderDetailItemDetail(int InvoiceLineId)
        {
            var data = new pos13_app_dataDataContext();

            var trnSalesOrderDetailItemDatailQ002 = (from sl in data.TrnSalesLines
                join im in data.MstItems on sl.ItemId equals im.Id
                join dt in data.MstDiscounts on sl.DiscountId equals dt.Id
                join tx in data.MstTaxes on sl.TaxId equals tx.Id
                join ut in data.MstUnits on sl.UnitId equals ut.Id
                where sl.Id == InvoiceLineId
                select new TrnSalesOrderDetailController()
                {
                    ItemId = sl.ItemId,
                    ItemDescription = im.ItemDescription,
                    Quantity = sl.Quantity,
                    UnitId = sl.UnitId,
                    Unit = ut.Unit,
                    Price = sl.Price,
                    DiscountId = sl.DiscountId,
                    DiscountRate = dt.DiscountRate,
                    DiscountAmount = sl.DiscountAmount,
                    NetPrice = sl.NetPrice,
                    Amount = sl.Amount,
                    TaxId = sl.TaxId,
                    Tax = tx.Tax,
                    TaxRate = tx.Rate,
                    TaxAmount = sl.TaxAmount
                }).FirstOrDefault();

            return trnSalesOrderDetailItemDatailQ002;
        }
        public void InserSalesOrderLine(
            int Id,
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
            )
        {
            var data = new pos13_app_dataDataContext();

            var SalesLine = new TrnSalesLine()
            {
                Id = Id,
                SalesId = SalesId,
                ItemId = ItemId,
                UnitId = UnitId,
                Price = Price,
                DiscountId = DiscountId,
                DiscountRate = DiscountRate,
                DiscountAmount = DiscountAmount,
                NetPrice = NetPrice,
                Quantity = Quantity,
                Amount = Amount,
                TaxId = TaxId,
                TaxRate = TaxRate,
                TaxAmount = TaxAmount,
                SalesAccountId = SalesAccountId,
                AssetAccountId = AssetAccountId,
                CostAccountId = CostAccountId,
                TaxAccountId = TaxAccountId,
                SalesLineTimeStamp = SalesLineTimeStamp,
                UserId = UserId,
                Preparation = Preparation
            };

            if (Id > 0)
            {
                var trnsalesline = data.TrnSalesLines.Single(i => i.Id == Id);

                trnsalesline.SalesId = SalesLine.SalesId;
                trnsalesline.ItemId = SalesLine.ItemId;
                trnsalesline.UnitId = SalesLine.UnitId;
                trnsalesline.Price = SalesLine.Price;
                trnsalesline.DiscountId = SalesLine.DiscountId;
                trnsalesline.DiscountRate = SalesLine.DiscountRate;
                trnsalesline.DiscountAmount = SalesLine.DiscountAmount;
                trnsalesline.NetPrice = SalesLine.NetPrice;
                trnsalesline.Quantity = SalesLine.Quantity;
                trnsalesline.Amount = SalesLine.Amount;
                trnsalesline.TaxId = SalesLine.TaxId;
                trnsalesline.TaxAmount = SalesLine.TaxAmount;
                trnsalesline.SalesAccountId = SalesLine.SalesAccountId;
                trnsalesline.AssetAccountId = SalesLine.AssetAccountId;
                trnsalesline.CostAccountId = SalesLine.CostAccountId;
                trnsalesline.TaxAccountId = TaxAccountId;
                trnsalesline.SalesLineTimeStamp = SalesLine.SalesLineTimeStamp;
                trnsalesline.UserId = SalesLine.UserId;
                trnsalesline.Preparation = SalesLine.Preparation;

                data.SubmitChanges();
            }
            else
            {
                var trnsalesline = new TrnSalesLine()
                {
                    SalesId = SalesLine.SalesId,
                    ItemId = SalesLine.ItemId,
                    UnitId = SalesLine.UnitId,
                    Price = SalesLine.Price,
                    DiscountId = SalesLine.DiscountId,
                    DiscountRate = SalesLine.DiscountRate,
                    DiscountAmount = SalesLine.DiscountAmount,
                    NetPrice = SalesLine.NetPrice,
                    Quantity = SalesLine.Quantity,
                    Amount = SalesLine.Amount,
                    TaxId = SalesLine.TaxId,
                    TaxAmount = SalesLine.TaxAmount,
                    SalesAccountId = SalesLine.SalesAccountId,
                    AssetAccountId = SalesLine.AssetAccountId,
                    CostAccountId = SalesLine.CostAccountId,
                    TaxAccountId = TaxAccountId,
                    SalesLineTimeStamp = SalesLine.SalesLineTimeStamp,
                    UserId = SalesLine.UserId,
                    Preparation = SalesLine.Preparation
                };

                data.TrnSalesLines.InsertOnSubmit(trnsalesline);
                data.SubmitChanges();
            };
        }
        public void DeleteSalesLine(int SalesLineId)
        {
            var data = new pos13_app_dataDataContext();
            var trnsalesline = data.TrnSalesLines.Single(i => i.Id == SalesLineId);

            data.TrnSalesLines.DeleteOnSubmit(trnsalesline);
            data.SubmitChanges();
        }
    }
}