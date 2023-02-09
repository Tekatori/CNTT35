using CNTT35.Data;
using CNTT35.ViewModel;
using System.Collections.Generic;

namespace CNTT35.Service.Service
{
    public interface IDonHangService
    {
        int findidGiamGia(string ma);
        DONHANG ThanhToanDonHang(int idkm, int idnd, string hoten, string diachigiao, string sdt, string email, string ghichu, GioHang gh, decimal tong, string hinhthuctt, string loaivc);
        List<Fn_GetCTSP_Result> GetCTDH(int id);
    }
}