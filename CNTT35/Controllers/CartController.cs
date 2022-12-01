using CNTT35.Session;
using CNTT35.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CNTT35.Controllers
{

    public class CartController : Controller
    {
        // GET: Cart   
        public ActionResult Index()
        {
            GioHang gh = (GioHang)Session["gh"];
            return View(gh);
        }
        public ActionResult Confirm()
        {
            return View();
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

            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Them(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DesSL(int id)
        {

            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Xoa(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DeleteSP(int id)
        {

            GioHang gh = (GioHang)Session["gh"];
            int kq = gh.Xoafull(id);
            Session["gh"] = gh;

            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public ActionResult GiamGia(FormCollection c)
        {
            string maGiamGia = c["magiam"].ToString().Trim();
            GioHang gh = (GioHang)Session["gh"];
            decimal kq = gh.TongThanhTienGiamGia(maGiamGia);
            Session["gh"] = gh;
            TempData["giamgia"] = kq;

            return RedirectToAction("Index", "Cart");
        }
        public ActionResult XoaGio()
        {
            GioHang gh = (GioHang)Session["gh"];
            gh.XoaGioHang();
            Session["gh"] = gh;
            return RedirectToAction("Index", "Cart");
        }
    }
}