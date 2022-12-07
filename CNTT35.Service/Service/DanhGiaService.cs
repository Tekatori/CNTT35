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
            using (var db = new QL_PHANBONEntities())
            {
                try
                {
                    var listdanhgia = from dg in db.PHANHOI
                                      join ng in db.NGUOIDUNG on dg.IDND equals ng.IDND
                                      where dg.IDSP == id
                                      select dg;

                    
                    return listdanhgia.ToList();
                }
                catch
                {
                    return null;
                }
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
    }
}
