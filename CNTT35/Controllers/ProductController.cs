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
            var sp = _productService.GetAllSanPham();

            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 12;
            return View(sp.ToPagedList((int)page, (int)pagesize));
        }

        public ActionResult Detail(string id)
        {
            var sp = _productService.GetSANPHAM(int.Parse(id));
            return View(sp);
        }
        [HttpPost]
        public ActionResult Search(int? page, int? pagesize,FormCollection c)
        {
            string tim = c["st"].ToString();
            var sp = _productService.SearchSP(tim);
            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 100;

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
            GioHang gh = (GioHang)Session["gh"];
            if (gh == null)
                gh = new GioHang();
            int kq = gh.Them(int.Parse(id),int.Parse(soluong));
            Session["gh"] = gh;
            return RedirectToAction("Detail/"+id,"Product");
        }
    }
}