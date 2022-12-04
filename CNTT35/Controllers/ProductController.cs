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

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public ActionResult Index(int? page, int? pagesize)
        {
            Session["giamgia"] = null;
            var  sp = _productService.GetAllSanPham();  
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
            var sp = _productService.GetSANPHAM(int.Parse(id));
            Decimal giakm = _productService.GetGiaTienSPKM(int.Parse(id));
            TempData["giakm"] = giakm;
            List<GetTop10Category_Result> top10KMCate = _productService.GetTop10SanPhamCategory((int)sp[0].IDDM);
            ViewBag.top10KMCate = top10KMCate;

            return View(sp);
        }
        [HttpPost]
        public ActionResult Search(int? page, int? pagesize,FormCollection c)
        {
            Session["giamgia"] = null;
            string tim = c["search"].ToString();
            var sp = _productService.SearchSP(tim);
            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 100;
            List<GetTop10_Result> top10Random = _productService.GetTop10SanPham();
            ViewBag.top10Random = top10Random;

            //List<GetTop10KM_Result> top10KM = _productService.GetTop10SanPhamKM();
            //ViewBag.top10KM = top10KM;

            ViewBag.top10KM = null;
            return View("Index",sp.ToPagedList((int)page, (int)pagesize));
        }

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
            
        }
    }
}