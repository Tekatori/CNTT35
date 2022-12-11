using CNTT35.Data;
using CNTT35.Service.Model;
using System.Collections.Generic;

namespace CNTT35.Service.Service
{
    public interface IAccountService
    {
        NGUOIDUNG CheckLogin(string username, string pass);
        int ThemAccount(string username, string password, string phone, string email);
        List<AccountInfo> GetAccount(int id);
        int ChangeInfo(int id, string hoten, string diachi, string sdt, string email);
        int ChangePassword(int id, string passOld, string passNew, string rePassNew);
        NGUOIDUNG GetND(int id);
        List<LichSuMuaHang_Result> GetLichSuDonHang(int id);
        int ThemAccountFB(string username, string email, string hoten);
        NGUOIDUNG CheckLoginFB(string username);
        List<getDiaChiKhac_Result> GetDiachiKhac(int id);
        int insertDiaChi(int idnd, string hoten, string sdt, string diachi);
        int UpdateDiaChi(int iddc, string hoten, string sdt, string diachi);
        int XoaDiaChi(int iddc);
        DIACHIKHAC DiachiKhac(int id);
        NGUOIDUNG CheckEmail(string Email);
    }
}