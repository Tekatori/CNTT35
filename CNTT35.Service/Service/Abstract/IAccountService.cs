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
    }
}