using CNTT35.Data;
using System.Collections.Generic;

namespace CNTT35.Service.Service
{
    public interface IDanhGiaService
    {
        List<PHANHOI> GetAllDanhGiaSP(int id);
        double TBRate(int id);
    }
}