using CNTT35.Data;
using CNTT35.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Service
{
    public class DanhGiaService : IDanhGiaService
    {
        public List<PHANHOI> GetAllDanhGiaSP(int id)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {

                    var listdanhgia = db.PHANHOI.Where(t => t.IDSP == id).ToList();

                    return listdanhgia.ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public double TBRate(int id)
        {
            using (var db = new QL_PHANBONEntities())
            {
                try
                {
                    var listdanhgia = from dg in db.PHANHOI
                                      join ng in db.NGUOIDUNG on dg.IDND equals ng.IDND
                                      where dg.IDSP == id
                                      select dg;
                    if(listdanhgia.Count() <= 0)
                        return 0;
                    double sum = 0;
                    foreach(var d in listdanhgia)
                    {
                        sum = sum +(double) d.RATE;
                    }
                    return sum / listdanhgia.Count(); 

                }
                catch
                {
                    return 0;
                }
            }
        }
        public int PhanHoiSP(int idsp, int IDND, string HoTenDG, string NOIDUNG, string CHATLUONGSANPHAM, string DUNGVOIMOTA,int Rate)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    PHANHOI ph = new PHANHOI();
                    ph.IDSP = idsp;
                    ph.DUNGVOIMOTA = DUNGVOIMOTA;
                    ph.HOTENDG = HoTenDG;
                    ph.RATE = Rate;
                    ph.CHATLUONGSANPHAM = CHATLUONGSANPHAM;
                    ph.IDND = IDND;
                    ph.NOIDUNG = NOIDUNG;
                    ph.NGAYPHANHOI = DateTime.Now;
                    db.PHANHOI.Add(ph);
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
