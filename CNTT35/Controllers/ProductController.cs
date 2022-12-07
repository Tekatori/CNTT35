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
        public ActionResult Index(int? page, int? pagesize)
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
            return View(sp.ToPagedList((int)page, (int)pagesize));
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
            ViewBag.ttsp = sp;

            var jj = DanhGiaService.TBRate(id);
            ViewBag.ttsp = jj;
            return PartialView(dg.ToPagedList((int)page, (int)pagesize));
        }


        [HttpPost]
        public ActionResult Search(int? page, int? pagesize,FormCollection c)
        {

            Session["giamgia"] = null;
            Session["maGiamGia"] = null;
            string tim = c["st"].ToString();
            var sp = _productService.SearchSP(tim);
            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 100;
            List<GetTop10_Result> top10Random = _productService.GetTop10SanPham();
            ViewBag.top10Random = top10Random;
            ViewBag.top10KM = null;
            return View("Index",sp.ToPagedList((int)page, (int)pagesize));
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