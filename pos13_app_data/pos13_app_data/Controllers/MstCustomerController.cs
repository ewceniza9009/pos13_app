using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data.Controllers
{
    public class MstCustomerController
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public int TermId { get; set; }
        public string Term { get; set; }
        public List<MstCustomerController> CustomerCombo()
        {
            var pos13 = new pos13_app_dataDataContext();

            var data = from i in pos13.MstCustomers
                join tm in pos13.MstTerms on i.TermId equals tm.Id
                where i.IsLocked 
                orderby i.Customer ascending
                select new MstCustomerController()
                {
                    Id = i.Id,
                    Customer = i.Customer,
                    TermId = i.TermId,
                    Term = tm.Term
                };

            return data.ToList();
        }
    }
}