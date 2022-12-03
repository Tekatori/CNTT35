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
using CNTT35.Data;
using System.Xml.Linq;

namespace CNTT35.Controllers
{
    public class HomeController : Controller
    {
        IProductService _productService;
        IAccountService _accountService;

        public HomeController(IProductService productService, IAccountService account)
        {
            _productService = productService;
            _accountService = account;
        }
        public ActionResult Index()
        {
            Session["giamgia"] = null;
            var sp = _productService.GetAllSanPham();
            List<GetTop10_Result> top10 = _productService.GetTop10SanPham();
            ViewBag.Greeting = top10;
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
            var nd2 = LoginSession.GetSessionInfoLogin();
            var nd = _accountService.GetAccount(nd2.IDND);
            return View(nd);
        }
        [HttpPost]
        public ActionResult Login(FormCollection c)
        {
            string username = c["username"].ToString();
            string password = c["password"].ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var res = _accountService.CheckLogin(username, password);
                if (res != null)
                {
                    //cooki
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddDays(2), false, username, FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);

                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    //session
                    LoginSession.createSession(res);
                    Session["username"] = res;
                    return RedirectToAction("Index", "Home");


                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        [HttpPost]
        public ActionResult Index(FormCollection c)
        {
            string username = c["tenND"].ToString();
            string password = c["pass"].ToString();
            string email = c["Email"].ToString();
            string phone = c["SDT"].ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int res2 = _accountService.ThemAccount(username, password, phone, email);
                if (res2 == 1)
                {
                    var res = _accountService.CheckLogin(username, password);
                    if (res != null)
                    {
                        //cooki
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddDays(2), false, username, FormsAuthentication.FormsCookiePath);

                        string encTicket = FormsAuthentication.Encrypt(ticket);

                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                        //session
                        LoginSession.createSession(res);
                        Session["username"] = res;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { ac = "error" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { ac = "error" });
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
        //ChangeInfo
        [HttpPost]
        public ActionResult ChangeInfo(FormCollection c)
        {
            var id = LoginSession.GetSessionInfoLogin().IDND;
            string hoten = c["hoten"].ToString();
            string sonha = c["sonha"].ToString();
            string xa = c["xa"].ToString();
            string huyen = c["huyen"].ToString();
            string tinh = c["tinh"].ToString();
            string email = c["email"].ToString();
            string sdt = c["sdt"].ToString();
            string dichi = sonha + "," + xa + "," + huyen + "," + tinh;
            int res = _accountService.ChangeInfo(id, hoten, dichi, sdt, email);
            if (res == 0)
            {
                return RedirectToAction("Account/" + id, "Home", new { ac2 = "error2" });
            }
            else
            {
                return RedirectToAction("Account/" + id, "Home");
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection c)
        {
            var id = LoginSession.GetSessionInfoLogin().IDND;
            string passold = c["passcu"].ToString();
            string passnew = c["passnew"].ToString();
            string repassnew = c["repassnew"].ToString();

            int res = _accountService.ChangePassword(id, passold,passnew,repassnew);
            if (res == 0)
            {
                return RedirectToAction("Account/" + id, "Home", new { ac3 = "error3" });
            }
            else
            {
                return RedirectToAction("Account/" + id, "Home");
            }
        }

    }
}