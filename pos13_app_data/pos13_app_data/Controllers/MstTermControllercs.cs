using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstTermController
    {
        public int Id { get; set; }
        public string Term { get; set; }

        public List<MstTermController> TermCombo()
        {     
            var pos13 = new pos13_app_dataDataContext();

            var data = from i in pos13.MstTerms
                where i.Term != null
                orderby i.Term ascending
                       select new MstTermController()
                {
                    Id = i.Id,
                    Term = i.Term
                };

                return data.ToList();
        }
    }
}