using CNTT35.Data;
using CNTT35.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Service
{
    public class DonHangService
    {
        public DonHangService()
        {
                
        }
        public int ThanhToanDonHang()
        {
            using (var db = new QL_PHANBONEntities())
            {


                return 0;
            }
        }
        public int NhapDonHang(int IDKM,int IDND,GioHang gh,decimal tong,string hinhthuc)
        {
            using (var db = new QL_PHANBONEntities())
            {
                var donhang = new DONHANG();
                donhang.IDKM = IDKM;
                donhang.IDND = IDND;
                donhang.NGAYLAP = DateTime.Now;
                db.DONHANG.Add(donhang);
                db.SaveChanges();
                foreach (CartItem ct in gh.ds)
                {
                    CTDONHANG ctdh = new CTDONHANG();
                    ctdh.IDDH = donhang.IDDH;
                    ctdh.SOLUONGSP = ct.iSoLuong;
                    ctdh.IDSP = ct.iMaSP;
                    db.CTDONHANG.Add(ctdh);
                    db.SaveChanges();
                }

                return 0;
            }
        }



    }
}
