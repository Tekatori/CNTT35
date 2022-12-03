using CNTT35.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Service
{
    public class ProductService : IProductService
    {
        public ProductService()
        {
        }
        public List<SANPHAM> GetAllSanPham()
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.SANPHAM.ToList();
            }
        }
        public List<SANPHAM> GetSANPHAM(int id)
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.SANPHAM.Where(t => t.IDSP == id).ToList();
            }
        }
        public List<SANPHAM> GetSanPhamHatGiong(int id)
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.SANPHAM.Where(t => t.IDDM == id).ToList();
            }
        }
        public List<SANPHAM> SearchSP(string tensp)
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.SANPHAM.Where(t => t.TENSP.Contains(tensp)).ToList();
            }
        }
        public List<GetTop10_Result> GetTop10SanPham()
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.GetTop10().ToList();
            }
        }
        public List<GetTop10KM_Result> GetTop10SanPhamKM()
        {
            using (var db = new QL_PHANBONEntities())
            {
                return db.GetTop10KM().ToList();
            }
        }
        public Decimal GetGiaTienSPKM(int id)
        {
            using (var db = new QL_PHANBONEntities())
            {
                try
                {
                    var g = db.SANPHAMKHUYENMAI.First(t => t.IDSP == id).GIATIENKM;
                    if (g != null)
                    {
                        return (decimal)g;
                    }
                    return 0;
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
