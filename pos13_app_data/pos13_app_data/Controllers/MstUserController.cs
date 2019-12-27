using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using pos13_app_data.Data;

namespace pos13_app_data
{
    public class MstUserController
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public List<MstUserController> UserCombo()
        {
            var pos13 = new pos13_app_dataDataContext();

            var data = from i in pos13.MstUsers
                where i.IsLocked
                orderby i.FullName ascending
                select new MstUserController()
                {
                    Id = i.Id,
                    FullName = i.FullName
                };

            return data.ToList();
        }

        public string getUserName(int id)
        {
            pos13_app_dataDataContext pos13 = new pos13_app_dataDataContext();

            string UserFullName = (from i in pos13.MstUsers
                                   where i.Id == id
                                   select i.FullName).Single();

            return UserFullName;

        }
        public int getUserId(string username, string password)
        {
            pos13_app_dataDataContext pos13 = new pos13_app_dataDataContext();
            int retId;

            try
            {
                retId = (from i in pos13.MstUsers
                         where i.UserName == username.Trim() && i.Password == password.Trim()
                         select i.Id).Single();
            }
            catch
            {
                retId = 0;
            }


            return retId;
        }

        public Boolean isLogin(string username,string password)
        {
            pos13_app_dataDataContext pos13 = new pos13_app_dataDataContext();
            int retId;

            try
            {
                retId = (from i in pos13.MstUsers
                            where i.UserName == username.Trim() && i.Password == password.Trim()
                            select i.Id).Single();
            }
            catch
            {
                retId = 0;
            }

            return retId > 0; ;
        }

    }
}