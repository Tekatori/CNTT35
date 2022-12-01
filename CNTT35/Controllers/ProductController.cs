using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using CNTT35.Service.Service;
using CNTT35.Data;

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
        //public ActionResult Index(int? page, int? pagesize,string id)
        //{
        //    var sp = _productService.GetSanPhamHatGiong(int.Parse(id));
        //    if (page == null)
        //        page = 1;
        //    if (pagesize == null)
        //        pagesize = 12;
        //    return View(sp.ToPagedList((int)page, (int)pagesize));
        //}
    }
}