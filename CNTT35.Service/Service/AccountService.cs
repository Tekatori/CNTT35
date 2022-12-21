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
                              NGAYDK = (DateTime)kh.NGAYDK,
                              Password = person.MATKHAU
                                
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
                    accountInfo.Password = item.Password;
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
        public NGUOIDUNG CheckLoginFB(string username)
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.NGUOIDUNG.Where(t => t.TENND == username || t.EMAIL == username).FirstOrDefault();
            }
        }
        public NGUOIDUNG CheckEmail(string Email)
        {
                using (var db = new QL_PHANBONEntities())
                {
                    return db.NGUOIDUNG.Where(t => t.EMAIL == Email).FirstOrDefault();
                  
                }   
        }



        public NGUOIDUNG GetND(int id)
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.NGUOIDUNG.FirstOrDefault(t => t.IDND == id);
            }
        }
        public int ChangeInfo(int id, string hoten, string diachi, string sdt)
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

                   return 1;
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
                        d = db.NGUOIDUNG.First(t => t.IDND == id && (t.MATKHAU == passOld || t.MATKHAU ==null));
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
        public int ThemAccountFB(string username,string email,string hoten)
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
                    nGUOIDUNG.MATKHAU =null;
                    db.NGUOIDUNG.Add(nGUOIDUNG);
                    db.SaveChanges();
                    KHACHHANG kHACHHANG = new KHACHHANG();
                    var nd = db.NGUOIDUNG.SingleOrDefault(t => t.TENND == username || t.EMAIL == email);
                    kHACHHANG.IDND = nd.IDND;
                    kHACHHANG.HOTEN = hoten;
                    kHACHHANG.SDT = null;
                    kHACHHANG.NGAYDK = DateTime.Now.Date;
                    kHACHHANG.DIACHI = null;
                    db.KHACHHANG.Add(kHACHHANG);
                    db.SaveChanges();
                    return 1;
                }
            }

        }
   
        public List<LichSuMuaHang_Result> GetLichSuDonHang(int id)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    var ls = db.LichSuMuaHang(id);
                    return ls.ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public List<getDiaChiKhac_Result> GetDiachiKhac(int id)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    var ls = db.getDiaChiKhac(id);
                    return ls.ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public DIACHIKHAC DiachiKhac(int id)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    var ls = db.DIACHIKHAC.First(t=>t.IDDC == id);
                    return ls;
                }
            }
            catch
            {
                return null;
            }
        }
        public int insertDiaChi(int idnd,string hoten,string sdt,string diachi)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    DIACHIKHAC dc = new DIACHIKHAC();
                    dc.IDND = idnd;
                    dc.HOTEN = hoten;
                    dc.SDT = sdt;
                    dc.DIACHI = diachi;
                    db.DIACHIKHAC.Add(dc);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }
        public int UpdateDiaChi(int iddc, string hoten, string sdt, string diachi)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    var dc = db.DIACHIKHAC.First(t => t.IDDC == iddc);
                    dc.HOTEN = hoten;
                    dc.SDT= sdt;
                    dc.DIACHI = diachi;
                    db.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }
        public int XoaDiaChi(int iddc)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    var dc = db.DIACHIKHAC.First(t => t.IDDC == iddc);
                    db.DIACHIKHAC.Remove(dc);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        public List<getPhanHoiID_Result> GetallPH(int id)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    return db.getPhanHoiID(id).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public int XoaPhanHoi(int id)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    var dc = db.PHANHOI.First(t => t.IDPHANHOI == id);
                    db.PHANHOI.Remove(dc);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }
        public int UpdatePhanHoi(int id, string hoten, string NoiDung, string chatluong,string dungvoimota,int rate)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    var dc = db.PHANHOI.FirstOrDefault(t => t.IDPHANHOI == id);
                    dc.HoTenDG = hoten;
                    dc.NOIDUNG = NoiDung;
                    dc.CHATLUONGSANPHAM = chatluong;
                    dc.DUNGVOIMOTA = dungvoimota;
                    dc.RATE = rate;
                    db.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
