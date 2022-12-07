using CNTT35.Data;
using CNTT35.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Service
{
    public class AccountService : IAccountService
    {
        public AccountService()
        {

        }
        public List<AccountInfo> GetAccount(int id)
        {
            using (var db = new QL_PHANBONEntities())
            {
                var res = from person in db.NGUOIDUNG
                          join kh in db.KHACHHANG on person.IDND equals kh.IDND
                          where kh.IDND == id
                          select new AccountInfo()
                          {
                              IDND = kh.IDND,
                              TENND = person.TENND,
                              EMAIL = person.EMAIL,
                              HOTEN = kh.HOTEN,
                              SDT = kh.SDT,
                              DIACHI = kh.DIACHI,
                              MATKHAU = person.MATKHAU,
                              NGAYDK = (DateTime)kh.NGAYDK
                          };
                List<AccountInfo> accountInfos = new List<AccountInfo>();
                foreach (var item in res)
                {
                    AccountInfo accountInfo = new AccountInfo();
                    accountInfo.IDND = item.IDND;
                    accountInfo.TENND = item.TENND;
                    accountInfo.EMAIL = item.EMAIL;
                    accountInfo.HOTEN = item.HOTEN;
                    accountInfo.SDT = item.SDT;
                    accountInfo.DIACHI = item.DIACHI;
                    accountInfo.MATKHAU = item.MATKHAU;
                    accountInfo.NGAYDK = item.NGAYDK;
                    accountInfos.Add(accountInfo);
                }
                return accountInfos;
            }
        }
        public NGUOIDUNG CheckLogin(string username, string pass)
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.NGUOIDUNG.Where(t => (t.TENND == username || t.EMAIL == username) && t.MATKHAU == pass).FirstOrDefault();
            }
        }
        public NGUOIDUNG GetND(int id)
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.NGUOIDUNG.FirstOrDefault(t => t.IDND == id);
            }
        }
        public int ChangeInfo(int id, string hoten, string diachi, string sdt, string email)
        {
            using (var db = new QL_PHANBONEntities())
            {
                NGUOIDUNG d = null;
                try
                {
                    d = db.NGUOIDUNG.First(t => t.IDND == id);
                }
                catch
                {
                    d = null;
                }
                if (d == null)
                {
                    return 0;
                }
                else
                {
                    var kh = db.KHACHHANG.First(t => t.IDND == id);
                    kh.HOTEN = hoten;
                    kh.DIACHI = diachi;
                    kh.SDT = sdt;
                    db.SaveChanges();

                    NGUOIDUNG nGUOIDUNG = null;
                    try
                    {
                        nGUOIDUNG = db.NGUOIDUNG.First(t => t.EMAIL == email);
                    }
                    catch
                    {
                        nGUOIDUNG = null;
                    }
                    if (nGUOIDUNG != null)
                    {
                        return 0;
                    }
                    else
                    {
                        d.EMAIL = email;
                        db.SaveChanges();
                        return 1;
                    }
                }

            }
        }
        public int ChangePassword(int id, string passOld, string passNew, string rePassNew)
        {
            using (var db = new QL_PHANBONEntities())
            {
                if (passNew == rePassNew)
                {
                    NGUOIDUNG d = null;
                    try
                    {
                        d = db.NGUOIDUNG.First(t => t.IDND == id && t.MATKHAU == passOld);
                    }
                    catch
                    {
                        d = null;
                    }
                    if (d == null)
                    {
                        return 0;
                    }
                    else
                    {
                        d.MATKHAU = passNew;
                        db.SaveChanges();
                        return 1;
                    }
                }
                else
                    return 0;
            }
        }
        public int ThemAccount(string username, string password, string phone, string email)
        {
            using (var db = new QL_PHANBONEntities())
            {
                NGUOIDUNG d = null;
                try
                {
                    d = db.NGUOIDUNG.First(t => t.TENND == username || t.EMAIL == email);
                }
                catch
                {
                    d = null;
                }
                if (d != null)
                {
                    return 0;
                }
                else
                {
                    NGUOIDUNG nGUOIDUNG = new NGUOIDUNG();
                    nGUOIDUNG.TENND = username;
                    nGUOIDUNG.EMAIL = email;
                    nGUOIDUNG.PHANQUYEN = 0;
                    nGUOIDUNG.MATKHAU = password;
                    db.NGUOIDUNG.Add(nGUOIDUNG);
                    db.SaveChanges();
                    KHACHHANG kHACHHANG = new KHACHHANG();
                    var nd = db.NGUOIDUNG.SingleOrDefault(t => t.TENND == username || t.EMAIL == email);
                    kHACHHANG.IDND = nd.IDND;
                    kHACHHANG.HOTEN = null;
                    kHACHHANG.SDT = phone;
                    kHACHHANG.NGAYDK = DateTime.Now.Date;
                    kHACHHANG.DIACHI = null;
                    db.KHACHHANG.Add(kHACHHANG);
                    db.SaveChanges();
                    return 1;
                }                             
            }

        }
    }
}
