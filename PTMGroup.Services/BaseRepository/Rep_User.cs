using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTMGroup.DataLayer.Domain;
using PTMGroup.DataLayer.ViewModels;

namespace PTMGroup.Services.BaseRepository
{
    public class Rep_User
    {
        private readonly PTMEntities  db = new PTMEntities();


        public Rep_User()
        {

        }

        public Model_AccountInfo GetInfoForNavbar(string Username)
        {
            var q = db.Tbl_Login.Where(a => a.Login_Email == Username || a.Login_Mobile == Username).SingleOrDefault();

            if (q != null)
            {
                Model_AccountInfo infoModel = new Model_AccountInfo();
                infoModel.Name = q.Tbl_User.User_Name + " " + q.Tbl_User.User_Family;
                infoModel.Role = q.Tbl_BaseRole.BR_Display;
                return infoModel;
            }
            else
            {
                return null;
            }
        }
    }
}
