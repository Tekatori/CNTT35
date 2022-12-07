namespace CNTT35.ViewModel
{
    public interface ICartItem
    {
        double dDonGia { get; set; }
        int iMaSP { get; set; }
        int iSoLuong { get; set; }
        string itags { get; set; }
        string sAnh { get; set; }
        string sTenSP { get; set; }
        double ThanhTien { get; }
    }
   
}