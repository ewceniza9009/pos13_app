using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstUnitController
    {
        public int Id { get; set; }
        public string Unit { get; set; }

        public List<MstUnitController> UnitCombo()
        {
            pos13_app_dataDataContext pos13 = new pos13_app_dataDataContext();

            var data = from i in pos13.MstUnits
                select new MstUnitController()
                {
                    Id = i.Id,
                    Unit = i.Unit
                };

            return data.ToList();

        }
    }
}