using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstItemGroupController
    {
        public int Id { get; set; }
        public string ItemGroup { get; set; }
        public string ImagePath { get; set; }
        public string KitchenReport { get; set; }
        public int EntryUserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public bool Islocked { get; set; }

        public List<MstItemGroupController> GetMstItemGroup()
        {
            var data = new pos13_app_dataDataContext();

            var mstItemGroup = from i in data.MstItemGroups
                           where i.IsLocked == true
                           select new MstItemGroupController
                           {
                               Id = i.Id,
                               ItemGroup = i.ItemGroup,
                               ImagePath = i.ImagePath,
                               KitchenReport = i.KitchenReport,
                               EntryUserId = i.EntryUserId,
                               EntryDateTime = i.EntryDateTime,
                               UpdateUserId = i.UpdateUserId,
                               UpdateDateTime = i.UpdateDateTime,
                               Islocked = i.IsLocked
                           };

            return mstItemGroup.ToList();
        }
        public int MinItemGroupId()
        {
            var data = new pos13_app_dataDataContext();
            var minItemGroupId = (from i in data.MstItemGroups
                                   select i.Id).FirstOrDefault();
            return minItemGroupId;
        }

    }
}