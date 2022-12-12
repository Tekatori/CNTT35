using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using CNTT35.Service.Service;
using CNTT35.Data;
using CNTT35.ViewModel;
using System.Drawing.Printing;
using System.Web.UI;
using CNTT35.Session;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Reflection;

namespace CNTT35.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        IProductService _productService;
        IDanhGiaService DanhGiaService;
        public ProductController(IProductService productService,IDanhGiaService DanhGia)
        {
            _productService = productService;
            DanhGiaService = DanhGia;
        }
        public ActionResult Index(int? page, int? pagesize,string sapxep ="",string loai="")
        {
            Session["giamgia"] = null;
            Session["maGiamGia"] = null;
            var sp = _productService.GetAllSanPham();

            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 12;
            //top 10 SP khuyen mai
            List<GetTop10KM_Result> top10KM = _productService.GetTop10SanPhamKM();
            ViewBag.top10KM = top10KM;

            //Top 10 ran
            List<GetTop10_Result> top10Random = _productService.GetTop10SanPham();
            ViewBag.top10Random = top10Random;
            //
            ViewBag.sapXepName = sapxep;
            
            ViewBag.hatgiong = loai;

            if (loai == "HatGiong")
            {
                if (sapxep == "a-z")
                    return View(sp.OrderBy(t => t.TENSP).Where(t=>t.IDDM==1).ToPagedList((int)page, (int)pagesize));
                else if (sapxep == "z-a")
                    return View(sp.OrderByDescending(t => t.TENSP).Where(t => t.IDDM == 1).ToPagedList((int)page, (int)pagesize));
                else
                    return View(sp.Where(t => t.IDDM == 1).ToPagedList((int)page, (int)pagesize));
            }
            else if(loai== "PhanBonla")
            {
                if (sapxep == "a-z")
                    return View(sp.OrderBy(t => t.TENSP).Where(t => t.IDDM == 2).ToPagedList((int)page, (int)pagesize));
                else if (sapxep == "z-a")
                    return View(sp.OrderByDescending(t => t.TENSP).Where(t => t.IDDM == 2).ToPagedList((int)page, (int)pagesize));
                else
                    return View(sp.Where(t => t.IDDM == 2).ToPagedList((int)page, (int)pagesize));
            }
            else if(loai == "PhanHuuCo")
            {
                if (sapxep == "a-z")
                    return View(sp.OrderBy(t => t.TENSP).Where(t => t.IDDM == 3).ToPagedList((int)page, (int)pagesize));
                else if (sapxep == "z-a")
                    return View(sp.OrderByDescending(t => t.TENSP).Where(t => t.IDDM == 3).ToPagedList((int)page, (int)pagesize));
                else
                    return View(sp.Where(t => t.IDDM == 3).ToPagedList((int)page, (int)pagesize));
            }
            else if(loai == "PhanVoCo")
            {
                if (sapxep == "a-z")
                    return View(sp.OrderBy(t => t.TENSP).Where(t => t.IDDM == 4).ToPagedList((int)page, (int)pagesize));
                else if (sapxep == "z-a")
                    return View(sp.OrderByDescending(t => t.TENSP).Where(t => t.IDDM == 4).ToPagedList((int)page, (int)pagesize));
                else
                    return View(sp.Where(t => t.IDDM == 4).ToPagedList((int)page, (int)pagesize));
            }    
            else
            {
                if (sapxep == "a-z")
                    return View(sp.OrderBy(t => t.TENSP).ToPagedList((int)page, (int)pagesize));
                else if (sapxep == "z-a")
                    return View(sp.OrderByDescending(t => t.TENSP).ToPagedList((int)page, (int)pagesize));
                else
                    return View(sp.ToPagedList((int)page, (int)pagesize));
            }    



           
        }

        public ActionResult Detail(string id)
        {
            Session["giamgia"] = null;
            Session["maGiamGia"] = null;
            var sp = _productService.GetSANPHAM(int.Parse(id));
            Decimal giakm = _productService.GetGiaTienSPKM(int.Parse(id));
            TempData["giakm"] = giakm;
            List<GetTop10Category_Result> top10KMCate = _productService.GetTop10SanPhamCategory((int)sp[0].IDDM);
            ViewBag.top10KMCate = top10KMCate;

            

            return View(sp);
        }
        public ActionResult DanhGia(int id, int? page, int? pagesize)
        {
            var dg = DanhGiaService.GetAllDanhGiaSP(id);
            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 3;
            var sp =  _productService.GetSP(id);
            ViewBag.sp = sp;

            var jj = DanhGiaService.TBRate(id);
            ViewBag.ttsp = jj;
            return PartialView(dg.ToPagedList((int)page, (int)pagesize));
        }
        [HttpPost]
        public ActionResult Index(FormCollection c, int? page, int? pagesize, string sapxep = "")
        {

            Session["giamgia"] = null;
            Session["maGiamGia"] = null;       
            string tim = c["st"].ToString();
            var sp = _productService.SearchSP(tim);
            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 120;
            //top 10 SP khuyen mai
            List<GetTop10KM_Result> top10KM = _productService.GetTop10SanPhamKM();
            ViewBag.top10KM = top10KM;
            //Top 10 ran
            List<GetTop10_Result> top10Random = _productService.GetTop10SanPham();
            ViewBag.top10Random = top10Random;
            //
            ViewBag.sapXepName = String.IsNullOrEmpty(sapxep) ? "z-a" : "a-z";
            if (sapxep == "a-z")
                return View(sp.OrderBy(t => t.TENSP).ToPagedList((int)page, (int)pagesize));
            else if (sapxep == "z-a")
                return View(sp.OrderByDescending(t => t.TENSP).ToPagedList((int)page, (int)pagesize));
            else
                return View(sp.ToPagedList((int)page, (int)pagesize));
        }
    
        //public ActionResult Index(int? page, int? pagesize,string id)
        //{
        //    var sp = _productService.GetSanPhamHatGiong(int.Parse(id));
        //    if (page == null)
        //        page = 1;
        //    if (pagesize == null)
        //        pagesize = 12;
        //    return View(sp.ToPagedList((int)page, (int)pagesize));
        //}
        public ActionResult ChonMua(int id)
        {
            GioHang gh = (GioHang)Session["gh"];
            if (gh == null)
                gh = new GioHang();
            int kq = gh.Them(id);
            Session["gh"] = gh;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ChonMua2(FormCollection c)
        {
            string soluong = c["quantity"].ToString();
            string id = TempData["id"].ToString();
            if (int.Parse(soluong) > 0)
            {
                GioHang gh = (GioHang)Session["gh"];
                if (gh == null)
                    gh = new GioHang();
                int kq = gh.Them(int.Parse(id), int.Parse(soluong));
                Session["gh"] = gh;
                return RedirectToAction("Detail/" + id, "Product");
            }
            else
            {
                return RedirectToAction("Detail/" + id, "Product");
            }    
        }
        [HttpPost]
        public ActionResult DanhGiaSP(FormCollection c, int id)
        {
            NGUOIDUNG nd2 = LoginSession.GetSessionInfoLogin();
            if (nd2 == null)
                return RedirectToAction("Detail/" + id, "Product", new { ac147 = "testdn" });
            int rate = 5;
            if (c["rating1"] != null)
            {
                rate = int.Parse(c["rating1"].ToString());
            }
            string NoiDung = c["NoiDung"].ToString();
            string TenDG = c["TenDG"].ToString();
            if (TenDG == null)
                TenDG = nd2.TENND;
            string ChatLuong = c["ChatLuong"].ToString();
            string DungVoiMota = c["DungVoiMota"].ToString();
            DanhGiaService.PhanHoiSP(id, nd2.IDND, TenDG, NoiDung, ChatLuong, DungVoiMota, rate);
            return RedirectToAction("Detail/" + id, "Product");
        }
        //SearchMoney1
        [HttpPost]
        public ActionResult SearchMoney(FormCollection c, int? page, int? pagesize)
        {
            try
            {
                int val1 = int.Parse(c["value1"].ToString());
                int val2 = int.Parse(c["value2"].ToString());
                var sp = _productService.SearchSPTheoDonGia(val1, val2);
                if (page == null)
                    page = 1;
                if (pagesize == null)
                    pagesize = 100;
                List<GetTop10_Result> top10Random = _productService.GetTop10SanPham();
                ViewBag.top10Random = top10Random;
                ViewBag.top10KM = null;
                Session["giamgia"] = null;
                return View("Index", sp.ToPagedList((int)page, (int)pagesize));
            }
            catch
            {
                Session["giamgia"] = null;
                Session["maGiamGia"] = null;
                var sp = _productService.GetAllSanPham();

                if (page == null)
                    page = 1;
                if (pagesize == null)
                    pagesize = 10;
                //top 10 SP khuyen mai
                List<GetTop10KM_Result> top10KM = _productService.GetTop10SanPhamKM();
                ViewBag.top10KM = top10KM;

                //Top 10 ran
                List<GetTop10_Result> top10Random = _productService.GetTop10SanPham();
                ViewBag.top10Random = top10Random;
                //
                return View(sp.ToPagedList((int)page, (int)pagesize));
            }
            
        }
    }
}