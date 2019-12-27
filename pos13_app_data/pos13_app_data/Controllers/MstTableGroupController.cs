using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstTableGroupController
    {

        public int Id { get; set; }
        public string TableGroup { get; set; }
        public int EntryUserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public List<MstTableGroupController> GetMstTableGroup()
        {
            var data = new pos13_app_dataDataContext();

            var mstTableGroup = from i in data.MstTableGroups
                                where i.TableGroup != "Walk-in" && i.TableGroup != "Delivery"
                                select new MstTableGroupController
                                {
                                    Id = i.Id,
                                    TableGroup = i.TableGroup,
                                    EntryUserId = i.EntryUserId,
                                    EntryDateTime = i.EntryDateTime,
                                    UpdateUserId = i.UpdateUserId,
                                    UpdateDateTime = i.UpdateDateTime
                                };

            return mstTableGroup.ToList();
        }

        public int MinTableGroupId()
        {
            var data = new pos13_app_dataDataContext();
            var minTableGroupId = (from i in data.MstTableGroups
                select i.Id).FirstOrDefault();
            return minTableGroupId;
        }
    }
}