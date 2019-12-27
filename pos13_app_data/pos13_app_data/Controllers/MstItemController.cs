using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstItemController
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        public int ItemGroupId { get; set; }
        public string ItemDescription { get; set; }
        public string Alias { get; set; }
        public string GenericName { get; set; }
        public string Category { get; set; }
        public int SalesAccountId { get; set; }
        public int OutTaxId { get; set; }
        public int UnitId { get; set; }
        public decimal Cost { get; set; }
        public decimal MarkUp { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public bool IsLocked { get; set; }

        public List<MstItemController> GetMstItem(int ItemGroupId)
        {
            var data = new pos13_app_dataDataContext();

            var mstItem = from i in data.MstItems
                join igi in data.MstItemGroupItems on i.Id equals igi.ItemId
                where i.IsLocked == true && igi.ItemGroupId == ItemGroupId
                select new MstItemController()
                {
                    Id = i.Id,
                    ItemGroupId = igi.ItemGroupId,
                    ItemCode = i.ItemCode,
                    Barcode = i.BarCode,
                    ItemDescription = i.ItemDescription,
                    Alias = i.Alias,
                    GenericName = i.GenericName,
                    Category = i.Category,
                    SalesAccountId = i.SalesAccountId,
                    OutTaxId = i.OutTaxId,
                    UnitId = i.UnitId,
                    Cost = i.Cost,
                    MarkUp = i.MarkUp,
                    Price= i.Price,
                    ImagePath=i.ImagePath,
                    IsLocked = i.IsLocked
                };
     

            return mstItem.ToList();
        }
        public List<Models.MstItem> ItemComboBox()
        {
            var data = new pos13_app_dataDataContext();

            var mstItem = from i in data.MstItems
                join tx in data.MstTaxes on i.OutTaxId equals tx.Id
                select new Models.MstItem()
                {
                    Id = i.Id,
                    ItemCode = i.ItemCode,
                    ItemDescription = i.ItemDescription,
                    Price = i.Price,
                    OutTaxId = tx.Id,
                    VatRate = tx.Rate
                };

            return mstItem.ToList();
        }
    }
}