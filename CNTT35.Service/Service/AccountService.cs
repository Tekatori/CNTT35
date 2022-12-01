using CNTT35.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Service
{
    public class AccountService : IAccountService
    {
        public AccountService()
        {

        }
        public NGUOIDUNG CheckLogin(string username, string pass)
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.NGUOIDUNG.Where(t => (t.TENND == username || t.EMAIL == username) && t.MATKHAU == pass).FirstOrDefault();
            }
        }
    }
}
