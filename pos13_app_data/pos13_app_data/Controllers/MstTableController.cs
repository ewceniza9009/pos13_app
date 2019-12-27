using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstTableController
    {
        public int Id { get; set; }
        public string TableCode { get; set; }
        public int TableGroupId { get; set; }
        public int? TopLocation { get; set; }
        public int? LeftLocation { get; set; }

        public List<MstTableController> GetMstTable(int TableGroupId)
        {
            var data = new pos13_app_dataDataContext();

            var mstTable = from i in data.MstTables
                           where i.TableGroupId == TableGroupId
                            select new MstTableController
                            {
                                Id = i.Id,
                                TableCode = i.TableCode,
                                TableGroupId = i.TableGroupId,
                                TopLocation = i.TopLocation,
                                LeftLocation = i.LeftLocation
                            };

            return mstTable.ToList();
        }
    }
}