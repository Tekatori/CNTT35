using CNTT35.Data;
using System;
using System.Collections.Generic;

namespace CNTT35.Service.Service
{
    public interface IProductService
    {
        List<SANPHAM> GetAllSanPham();
        List<SANPHAM> GetSANPHAM(int id);
        List<SANPHAM> GetSanPhamHatGiong(int id);
        List<SANPHAM> SearchSP(string tensp);
        List<GetTop10_Result> GetTop10SanPham();
        List<GetTop10KM_Result> GetTop10SanPhamKM();
        Decimal GetGiaTienSPKM(int id);
    }
}