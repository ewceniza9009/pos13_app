using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstTaxController
    {
        public int Id { get; set; }
        public string Tax { get; set; }
        public decimal Rate { get; set; }
        public string  Code { get; set; }
        public int AccountId { get; set; }

        public List<MstTaxController> TaxCombo()
        {
            pos13_app_dataDataContext pos13 = new pos13_app_dataDataContext();

            var data = from i in pos13.MstTaxes
                       select new MstTaxController()
                       {
                           Id = i.Id,
                           Tax = i.Tax,
                           Rate = i.Rate,
                           Code = i.Code,
                           AccountId = i.AccountId
                       };

            return data.ToList();

        }

    }
}