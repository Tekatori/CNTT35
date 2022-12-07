using CNTT35.Data;
using CNTT35.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Service
{
    public class DonHangService : IDonHangService
    {
        public DonHangService()
        {

        }
        public DONHANG ThanhToanDonHang(int idkm, int idnd, string hoten, string diachigiao, string sdt, string email, string ghichu, GioHang gh, decimal tong, string hinhthuctt, string loaivc)
        {
            using (var db = new QL_PHANBONEntities())
            {
                try
                {
                    DONHANG donhang = new DONHANG();
                    if (idkm != 0)
                        donhang.IDKM = idkm;
                    donhang.IDND = idnd;
                    donhang.HOTEN = hoten;
                    donhang.DIACHIGIAOHANG = diachigiao;
                    donhang.DIENTHOAI = sdt;
                    donhang.EMAIL = email;
                    donhang.GHICHU = ghichu;
                    donhang.NGAYLAP = DateTime.Now;
                    db.DONHANG.Add(donhang);
                    db.SaveChanges();
                    foreach (var item in gh.ds)
                    {
                        CTDONHANG ctdonhang = new CTDONHANG();
                        ctdonhang.IDDH = donhang.IDDH;
                        ctdonhang.IDSP = item.iMaSP;
                        ctdonhang.SOLUONGSP = item.iSoLuong;
                        db.CTDONHANG.Add(ctdonhang);
                        db.SaveChanges();
                    }
                    QLVANCHUYEN qlvanchuyen = new QLVANCHUYEN();
                    qlvanchuyen.IDDH = donhang.IDDH;
                    qlvanchuyen.LOAIVANCHUYEN = loaivc;
                    if (loaivc == "Tiết Kiệm")
                    {
                        qlvanchuyen.PHIVANCHUYEN = 35000;
                        qlvanchuyen.NGAYGIAOHANG = DateTime.Now.AddDays(7);
                    }
                    else
                    {
                        qlvanchuyen.PHIVANCHUYEN = 20000;
                        qlvanchuyen.NGAYGIAOHANG = DateTime.Now.AddDays(3);
                    }
                    db.QLVANCHUYEN.Add(qlvanchuyen);
                    db.SaveChanges();
                    THANHTOAN thanhToan = new THANHTOAN();
                    thanhToan.IDDH = donhang.IDDH;
                    thanhToan.HINHTHUCTT = hinhthuctt;
                    thanhToan.THANHTIEN = tong;
                    db.THANHTOAN.Add(thanhToan);
                    db.SaveChanges();
                    return donhang;
                }
                catch
                {
                    return null;
                }
            }
        }
        public int findidGiamGia(string ma)
        {
            using (var db = new QL_PHANBONEntities())
            {
                try
                {
                    var id = db.KHUYENMAI.FirstOrDefault().IDKM;
                    return id;
                }
                catch
                {
                    return 0;
                }
            }
        }

    }

}
