using CNTT35.Data;
using CNTT35.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNTT35.ViewModel
{
    public class CartItem : ICartItem
    {
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnh { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public string itags { get; set; }
        public double ThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public CartItem()
        {

        }
        QL_PHANBONEntities db = new QL_PHANBONEntities();
        ProductService ProductService = new ProductService();
        public CartItem(int id)
        {
            SANPHAM sanpham = db.SANPHAM.Single(n => n.IDSP == id);
            if (sanpham != null)
            {
                iMaSP = id;
                sTenSP = sanpham.TENSP;
                string[] arrListStr = sanpham.HINHANH.ToString().Split(',');
                sAnh = arrListStr[0].ToString();
                var res = ProductService.GetGiaTienSPKM(id);
                if(res == 0)
                    dDonGia = double.Parse(sanpham.DONGIA.ToString());
                else
                    dDonGia = double.Parse(res.ToString());
                itags = sanpham.MOTA;
                iSoLuong = 1;
            }
        }
        public CartItem(int id,int soluong)
        {
            SANPHAM sanpham = db.SANPHAM.Single(n => n.IDSP == id);
            if (sanpham != null)
            {
                iMaSP = id;
                sTenSP = sanpham.TENSP;
                string[] arrListStr = sanpham.HINHANH.ToString().Split(',');
                sAnh = arrListStr[0].ToString();
                var res = ProductService.GetGiaTienSPKM(id);
                if (res == 0)
                    dDonGia = double.Parse(sanpham.DONGIA.ToString());
                else
                    dDonGia = double.Parse(res.ToString());
                itags = sanpham.MOTA;
                iSoLuong = soluong;
            }
        }
    }

    public class GioHang
    {
        QL_PHANBONEntities db = new QL_PHANBONEntities();
        public List<CartItem> ds;
        public GioHang()
        {
            ds = new List<CartItem>();
        }
        public GioHang(List<CartItem> dsGH)
        {
            if (dsGH == null)
                ds = new List<CartItem>();
            else
                ds = dsGH;
        }
        public int SoMatHang()
        {
            int count = 0;
            if (ds == null)
                return count;
            foreach (var item in ds)
            {
                if(item.iSoLuong >0)
                {
                    count++;
                }
            }          
            return count;
        }
        public int TongSLHang()
        {
            int tong = 0;
            if (ds != null)
            {
                tong = ds.Sum(n => n.iSoLuong);
                return tong;
            }
            return 0;
        }
        public double TongThanhTien()
        {
            double tong = 0;
            if (ds != null)
            {
                tong = ds.Sum(n => n.ThanhTien);
                return tong;
            }
            return 0;
        }
        public Decimal TongThanhTienGiamGia(string MaGiamGia)
        {
            Decimal tong = Decimal.Parse(TongThanhTien().ToString());
            var giamgia = db.KHUYENMAI.FirstOrDefault(t=>t.MAKHUYENMAI == MaGiamGia);
            if (ds != null && giamgia !=null )
            {
                tong = tong - (decimal)giamgia.GIATIENKM;
                return tong;
            }
            return Decimal.Parse(TongThanhTien().ToString());
        }
        public int Them(int iMa)
        {
            CartItem sp = ds.Find(n => n.iMaSP == iMa);
            if (sp == null)
            {
                CartItem sanpham = new CartItem(iMa);
                if (sanpham == null)
                {
                    return -1;
                }
                ds.Add(sanpham);
            }
            else
            {
                sp.iSoLuong++;
            }
            return 1;
        }
        public int Them(int iMa,int soluong)
        {
            CartItem sp = ds.Find(n => n.iMaSP == iMa);
            if (sp == null)
            {
                CartItem sanpham = new CartItem(iMa, soluong);
                if (sanpham == null)
                {
                    return -1;
                }
                ds.Add(sanpham);
            }
            else
            {
                sp.iSoLuong++;
            }
            return 1;
        }
        public int Xoa(int iMa)
        {
            CartItem sp = ds.Find(n => n.iMaSP == iMa);
            if (sp != null)
            {
                CartItem sanpham = new CartItem(iMa);
                ds.Remove(sanpham);
                sp.iSoLuong--;
            }
            return 1;
        }
        public int Xoafull(int iMa)
        {
            CartItem sp = ds.Find(n => n.iMaSP == iMa);
            {
                CartItem sanpham = new CartItem(iMa);
                ds.Remove(sanpham);
                sp.iSoLuong = 0;
            }
            return 1;
        }
        public int XoaGioHang()
        {
            if (ds == null)
                return 1;
            else
            {
                ds.Clear();
                return 1;
            }
        }
    }
}