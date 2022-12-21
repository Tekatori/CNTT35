using CNTT35.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Service
{
    public class KhuyenMaiService : IKhuyenMaiService
    {
        public List<KHUYENMAI> GettAllKM()
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    return db.KHUYENMAI.ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public int updateSL(int id)
        {

            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    var dt = db.KHUYENMAI.FirstOrDefault(t => t.IDKM == id);
                    if (dt.SOLUONGKM == 0) return 0;
                    else
                    {
                        dt.SOLUONGKM = dt.SOLUONGKM - 1;
                        db.SaveChanges();
                        return 1;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
