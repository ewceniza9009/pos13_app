using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstDiscountController
    {
        public int Id { get; set; }
        public string Discount { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsVatExempt { get; set; }
        public bool IsVatExemp { get; set; }
        public bool IsDateScheduled { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool IsTimeScheduled { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public bool IsDayScheduled { get; set; }
        public bool DayMon { get; set; }
        public bool DayTue { get; set; }
        public bool DayWed { get; set; }
        public bool DayThu { get; set; }
        public bool DayFri { get; set; }
        public bool DaySat { get; set; }
        public bool DaySun { get; set; }
        public int EntryUserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public bool IsLocked { get; set; }
        public List<MstDiscountController> DiscountCombo(DateTime SalesDate,int ItemId)
        {
            var data = new pos13_app_dataDataContext();

            var mstDiscountQ002 = from i in data.MstDiscounts
                where (i.IsDateScheduled == false || (SalesDate >= i.DateStart && SalesDate <= DateEnd)) &&
                      (i.IsTimeScheduled == false || (DateTime.Now >= i.DateStart && DateTime.Now <= DateEnd)) &&
                      (i.IsDayScheduled == false || (String.Format("{0:ddd}", SalesDate) == "Mon" && i.DayMon)) &&
                      (i.IsDayScheduled == false || (String.Format("{0:ddd}", SalesDate) == "Tue" && i.DayTue)) &&
                      (i.IsDayScheduled == false || (String.Format("{0:ddd}", SalesDate) == "Wed" && i.DayWed)) &&
                      (i.IsDayScheduled == false || (String.Format("{0:ddd}", SalesDate) == "Thu" && i.DayThu)) &&
                      (i.IsDayScheduled == false || (String.Format("{0:ddd}", SalesDate) == "Fri" && i.DayFri)) &&
                      (i.IsDayScheduled == false || (String.Format("{0:ddd}", SalesDate) == "Sat" && i.DaySat)) &&
                      (i.IsDayScheduled == false || (String.Format("{0:ddd}", SalesDate) == "Sun" && i.DaySun)) &&
                      (data.MstDiscountItems.Count(i2 => i2.DiscountId == i.Id) == 0
                          ? 1
                          : (from i2 in data.MstDiscountItems
                              where i2.DiscountId == i.Id && i2.ItemId == ItemId
                              select i2.Id).Count()) > 0 &&
                      i.Discount != "Senior Citizen Discount"
                select new MstDiscountController()
                {
                    Id = i.Id,
                    Discount = i.Discount,
                    DiscountRate = i.DiscountRate
                };


            return mstDiscountQ002.ToList();
        }

    }
}