using CNTT35.Data;
using System.Collections.Generic;

namespace CNTT35.Service.Service
{
    public interface IProductService
    {
        List<SANPHAM> GetAllSanPham();
        List<SANPHAM> GetSANPHAM(int id);
        List<SANPHAM> GetSanPhamHatGiong(int id);
        List<SANPHAM> SearchSP(string tensp);
    }
}