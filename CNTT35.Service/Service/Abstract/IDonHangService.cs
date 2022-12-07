using CNTT35.Data;
using CNTT35.ViewModel;

namespace CNTT35.Service.Service
{
    public interface IDonHangService
    {
        int findidGiamGia(string ma);
        DONHANG ThanhToanDonHang(int idkm, int idnd, string hoten, string diachigiao, string sdt, string email, string ghichu, GioHang gh, decimal tong, string hinhthuctt, string loaivc);
    }
}