using CNTT35.Session;
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
            return View();
        }
        public ActionResult Confirm()
        {
            return View();
        }
    }
}