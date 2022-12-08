using CNTT35.Data;
using CNTT35.Service.Model;
using CNTT35.Service.Service;
using CNTT35.Session;
using CNTT35.ViewModel;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages;

namespace CNTT35.Controllers
{

    public class CartController : Controller
    {
        // GET: Cart
        IAccountService _accountService;
        IDonHangService DonHangService;
        public CartController(IAccountService account, IDonHangService donHangService)
        {
            _accountService = account;
            DonHangService = donHangService;
        }
        public ActionResult Index()
        {          
            GioHang gh = (GioHang)Session["gh"];
            if (gh == null)
                return RedirectToAction("Index", "Home", new { ac = "GioHangNull" });
            return View(gh);
        }
        public ActionResult Confirm()
        {
            GioHang gh = (GioHang)Session["gh"];

            if (gh == null)
                return RedirectToAction("Index", "Cart", new { ac147 = "error147" });
            int sl = gh.SoMatHang();
            if (sl <= 0)
            {
                return RedirectToAction("Index", "Cart", new { ac147 = "error147" });
            }
            else
            {
                NGUOIDUNG nd2 = LoginSession.GetSessionInfoLogin();
                if (nd2 == null)
                    return RedirectToAction("Index", "Cart", new { ac147 = "usernull" });
                var nd = _accountService.GetAccount(nd2.IDND);
                ViewBag.kh = nd;
                decimal tongtien = decimal.Parse(Session["tien"].ToString());
                double tienkm = double.Parse(Session["tienKM"].ToString());
                TempData["tienkm"] = tienkm;
                TempData["sum"] = tongtien;

                return View(gh);
            }
        }
        public ActionResult paymentSucces(FormCollection c)
        {
            try
            {
                GioHang gh = (GioHang)Session["gh"];
                var nd = LoginSession.GetSessionInfoLogin();
                string hoten = c["hoten"].ToString();
                string sonha = c["sonha"].ToString();
                string tinhthanh = c["tinhthanh"].ToString();
                string quanhuyen = c["quanhuyen"].ToString();
                string Phuongxa = c["Phuongxa"].ToString();
                string loaivc = c["flexRadioDefault"].ToString();
                string phone = c["phone"].ToString();
                string email = c["email"].ToString();
                string ghichu = c["ghichu"].ToString();
                string thanhtoan = c["thanhtoan"].ToString();
                string dichi = sonha + "," + Phuongxa + "," + quanhuyen + "," + tinhthanh;
                decimal tong = 0;
                int idkm = 0;
                if (Session["maGiamGia"] != null)
                {
                    string magg = Session["maGiamGia"].ToString();
                    idkm = DonHangService.findidGiamGia(magg);
                    tong = gh.TongThanhTienGiamGia(magg);
                }
                else
                {
                    idkm = 0;
                    tong = (decimal)gh.TongThanhTien();
                }    
                DONHANG dh = DonHangService.ThanhToanDonHang(idkm, nd.IDND, hoten, dichi, phone, email, ghichu, gh, tong, thanhtoan, loaivc);
                gh.XoaGioHang();
                clearSessionGiamGia();
                return View(dh);
            }
            catch
            {
                return RedirectToAction("Confirm", "Cart", new { ac = "error" });
            }
        }
        public ActionResult paymentHistory()
        {
            NGUOIDUNG nd2 = LoginSession.GetSessionInfoLogin();
            var listdh = _accountService.GetLichSuDonHang(nd2.IDND);

   
            return View(listdh);
        }
        public ActionResult ChonMua(int id)
        {
            GioHang gh = (GioHang)Session["gh"];
            if (gh == null)
                gh = new GioHang();
            int kq = gh.Them(id);
            Session["gh"] = gh;
            return RedirectToAction("/");
        }
        public ActionResult AddSL(int id)
        {
            clearSessionGiamGia();
            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Them(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DesSL(int id)
        {
            clearSessionGiamGia();
            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Xoa(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DeleteSP(int id)
        {
            clearSessionGiamGia();
            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Xoafull(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public ActionResult GiamGia(FormCollection c)
        {
            GioHang gh = (GioHang)Session["gh"];
            if (gh == null)
                return RedirectToAction("Index", "Home", new { ac = "GioHangNull" });
            string maGiamGia = c["magiam"].ToString().Trim();
            if (String.IsNullOrEmpty(maGiamGia) == false)
            {             
                decimal kq = 0;
                kq = gh.TongThanhTienGiamGia(maGiamGia);
                Session["gh"] = gh;
                Session["maGiamGia"] = maGiamGia;
                Session["giamgia"] = kq;
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult XoaGio()
        {
            clearSessionGiamGia();
            GioHang gh = (GioHang)Session["gh"];
            gh.XoaGioHang();
            Session["gh"] = gh;
            return RedirectToAction("Index", "Cart");
        }
        public void clearSessionGiamGia()
        {
            Session["giamgia"] = null;
            Session["maGiamGia"] = null;
        }
  
    }
}