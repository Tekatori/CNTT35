using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using CNTT35.Service.Service;
using System.Web.Security;
using CNTT35.Session;
using CNTT35.ViewModel;

namespace CNTT35.Controllers
{
    public class HomeController : Controller
    {
        IProductService _productService;
        IAccountService _accountService;

        public HomeController(IProductService productService,IAccountService account)
        {
            _productService = productService;
            _accountService = account;
        }
        public ActionResult Index()
        {
            var sp = _productService.GetAllSanPham();

            return View(sp);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Account()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection c)
        {
            string username = c["username"].ToString();
            string password = c["password"].ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                var res = _accountService.CheckLogin(username, password);
                if (res != null)
                {
                    //cooki
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddDays(2), false, username, FormsAuthentication.FormsCookiePath);

                    string  encTicket =FormsAuthentication.Encrypt(ticket);

                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    //session
                    LoginSession.createSession(res);
                    Session["username"] = res;
                    return RedirectToAction("Index", "Home");

                    
                }
                else
                {
                    return RedirectToAction("Index", "Product");
                }
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            LoginSession.clear();
            return RedirectToAction("Index", "Product");
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

    }
}