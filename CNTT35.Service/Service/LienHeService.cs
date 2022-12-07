using CNTT35.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Service
{
    public class LienHeService : ILienHeService
    {
        public LienHeService()
        {

        }
        public int insertLoiNhan(string hoten, string email, string loinhan)
        {
            try
            {
                using (var db = new QL_PHANBONEntities())
                {
                    LIENHE lienhe = new LIENHE();
                    lienhe.EMAIL = email;
                    lienhe.HOTEN = hoten;
                    lienhe.NOIDUNG = loinhan;
                    db.LIENHE.Add(lienhe);
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
