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
    }
}
