using CNTT35.Data;
using System.Collections.Generic;

namespace CNTT35.Service.Service
{
    public interface IKhuyenMaiService
    {
        List<KHUYENMAI> GettAllKM();
        int updateSL(int id);
    }
}