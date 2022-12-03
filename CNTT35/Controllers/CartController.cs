using CNTT35.Data;
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
        public CartController(IAccountService account)
        {
            _accountService = account;
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
            Session["giamgia"] = null;
            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Them(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DesSL(int id)
        {
            Session["giamgia"] = null;
            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Xoa(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DeleteSP(int id)
        {
            Session["giamgia"] = null;
            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Xoafull(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public ActionResult GiamGia(FormCollection c)
        {

            string maGiamGia = c["magiam"].ToString().Trim();
            if (String.IsNullOrEmpty(maGiamGia) == false)
            {
                GioHang gh = (GioHang)Session["gh"];
                decimal kq = 0;
                kq = gh.TongThanhTienGiamGia(maGiamGia);
                Session["gh"] = gh;
                Session["giamgia"] = kq;
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult XoaGio()
        {
            Session["giamgia"] = null;
            GioHang gh = (GioHang)Session["gh"];
            gh.XoaGioHang();
            Session["gh"] = gh;
            return RedirectToAction("Index", "Cart");
        }
  
    }
}