using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTT35.Service.Model
{
    public class DanhGiaViewModel
    {
        public int IDPHANHOI { get; set; }
        public int IDND { get; set; }
        public string TenND { get; set; }
        public int IDSP { get; set; }
        public string NOIDUNG {get; set; }
        public string CHATLUONGSANPHAM { get; set; }
        public string DUNGVOIMOTA { get; set; }
        public int RATE {get; set; }

        public DanhGiaViewModel()
        {

        }
    }
}
