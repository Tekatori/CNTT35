using CNTT35.Data;

namespace CNTT35.Service.Service
{
    public interface IAccountService
    {
        NGUOIDUNG CheckLogin(string username, string pass);
    }
}