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
using Facebook;
//using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.IO;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Ajax.Utilities;
using System.Web.Http.Controllers;

namespace CNTT35.Controllers
{

    public class HomeController : Controller
    {
        IProductService _productService;
        IAccountService _accountService;
        ILienHeService _lienHeService;
        IKhuyenMaiService _ikhuyenMaiService;
        string appid = string.Empty;
        string appsecret = string.Empty;
        public HomeController(IProductService productService, IAccountService account, ILienHeService lienHeService, IKhuyenMaiService ikhuyenMaiService)
        {
            _productService = productService;
            _accountService = account;
            _lienHeService = lienHeService;
            _ikhuyenMaiService = ikhuyenMaiService;
            var configuration = GetConfiguration();
            appid = "956578625314895";
            appsecret = "a3943837c2ea4f88b8b61aeb5e11003d";
        }
     

        public IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional:true, reloadOnChange: true);
            return builder.Build();
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = appid,
                client_secret = appsecret,
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }


        public ActionResult Index()
        {
            //_cartController.clearSessionGiamGia();
            Session["giamgia"] = null;
            Session["maGiamGia"] = null;
            var sp = _productService.Get30ProductRandom();
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
        public ActionResult Voucher()
        {
            var km = _ikhuyenMaiService.GettAllKM();

            return View(km);
        }
        public ActionResult addNewAdress(int? page, int? pagesize)
        {

            var nd2 = LoginSession.GetSessionInfoLogin();

            var dc = _accountService.GetDiachiKhac(nd2.IDND);
            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 10;
            return View(dc.ToPagedList((int)page, (int)pagesize));
        }
        [HttpPost]
        public ActionResult insertDC(FormCollection c)
        {           
            var nd2 = LoginSession.GetSessionInfoLogin();
            string hoten = c["edit-yourname"].ToString();
            string sdt = c["edit-numberphone"].ToString();
            string city = c["edit-city"].ToString();
            string district = c["edit-district"].ToString();
            string ward = c["edit-ward"].ToString();
            string address = c["edit-address"].ToString();
            string diachi = address + "," + ward + "," + district + "," + city;

            var dc = _accountService.insertDiaChi(nd2.IDND, hoten, sdt, diachi);
            if(dc==0 || sdt.Length <= 10)
                return RedirectToAction("addNewAdress", "Home", new { ac2 = "sdt" });
            return RedirectToAction("addNewAdress", "Home");
        }
        [HttpPost]
        public ActionResult QuenMK(FormCollection c)
        {
            string Email = c["EmailQuen"].ToString();

            try
            {
                var dc = _accountService.CheckEmail(Email);
                if (dc !=null)
                {

                    string fromMail = "banhangcntt35@gmail.com";
                    string fromPassword = "treisvognntadjxn";

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(fromMail);
                    message.Subject = "Quên Mật Khẩu Quản Lí Phân Bón";
                    message.To.Add(new MailAddress(dc.EMAIL));
                    message.Body = "<html><body> Email của bạn là :" + dc.EMAIL + " <br> Username của bạn là :" + dc.TENND + "<br> Password của bạn là : " + dc.MATKHAU + "</body></html>";
                    message.IsBodyHtml = true;

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromMail, fromPassword),
                        EnableSsl = true,
                    };

                    smtpClient.Send(message);
                    return RedirectToAction("Index", "Home", new { ac = "guimail" });
                }
                return RedirectToAction("Index", "Home", new { ac = "Emailnot" });
            }
            catch
            {
                return RedirectToAction("Index", "Home", new { ac = "Emailnot" });
            }       
        }

        [HttpPost]
        public ActionResult EditDC(FormCollection c,int id)
        {
            string hoten = c["update-yourname"].ToString();
            string sdt = c["update-numberphone"].ToString();
            string city = c["update-city"].ToString();
            string district = c["update-district"].ToString();
            string ward = c["update-ward"].ToString();
            string address = c["update-address"].ToString();
            string diachi = address + "," + ward + "," + district + "," + city;

            var dc = _accountService.UpdateDiaChi(id, hoten, sdt, diachi);
            if (dc == 0 || sdt.Length <=10)
                return RedirectToAction("addNewAdress", "Home", new { ac2 = "sdt" });
            return RedirectToAction("addNewAdress", "Home");
        }
        [HttpPost]
        public ActionResult EditPhanHoi(FormCollection c, int id)
        {
       
            string hoten = c["update-yourname"].ToString();
            string NoiDung = c["update-content"].ToString();
            string chatluong= c["update-quality"].ToString();
            string dungvoimota = c["update-describe"].ToString();
            //string rate = c["update-star"].ToString();
            string rate = c[id.ToString()].ToString();
            var dc = _accountService.UpdatePhanHoi(id, hoten, NoiDung, chatluong, dungvoimota, int.Parse(rate));
            if (dc == 0)
                return RedirectToAction("xemDanhGia", "Home", new { ac2 = "updatethatbai" });
            return RedirectToAction("xemDanhGia", "Home");
        }
        public ActionResult Account()
        {
            var nd2 = LoginSession.GetSessionInfoLogin();
            var nd = _accountService.GetAccount(nd2.IDND);

         
            return View(nd);
        }
        public ActionResult xemDanhGia(int? page, int? pagesize)
        {
            var nd = LoginSession.GetSessionInfoLogin();

            var dg = _accountService.GetallPH(nd.IDND);
            if (page == null)
                page = 1;
            if (pagesize == null)
                pagesize = 10;



            if (dg!=null)
                return View(dg.ToPagedList((int)page, (int)pagesize));
            return View();
        }
        public ActionResult XoaPH(int id)
        {
            var dc = _accountService.XoaPhanHoi(id);
            if (dc == 0)
                return RedirectToAction("xemDanhGia", "Home", new { ac2 = "trungdiachi" });
            return RedirectToAction("xemDanhGia", "Home");
        }
        public ActionResult XoaDC(int id)
        {
            var dc = _accountService.XoaDiaChi(id);
            if (dc == 0)
                return RedirectToAction("addNewAdress", "Home", new { ac2 = "trungdiachi" });
            return RedirectToAction("addNewAdress", "Home");
        }
   
        [HttpPost]
        public ActionResult Login(FormCollection c)
        {
            string username = c["username"].ToString();
            string password = c["password"].ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Index", "Home", new { ac = "tk" });
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
                    return Redirect("/");


                }
                else
                {
                    return RedirectToAction("Index", "Home", new { ac = "tk" });
                }
            }
        }
        //[AllowAnonymous]
        //public ActionResult LoginFB()
        //{
        //    var fb = new FacebookClient();
        //    var loginUrl = fb.GetLoginUrl(new
        //    {
        //        client_id = "1855259208191738",
        //        client_secret = "78550ec0c934b421d64b12d23ac7ca6d",
        //        redirect_uri = RedirectUri.AbsoluteUri,
        //        response_type = "code",
        //        scope = "email"
        //    });


        //    return Redirect(RedirectUri.AbsoluteUri);
        //}

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = appid,
                client_secret = appsecret,
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            fb.AccessToken = accessToken;

            if (!string.IsNullOrEmpty(accessToken))
            {
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string username = me.email;
                string middle_name = me.middle_name;
                string firstname = me.first_name;
                string lastname = me.last_name;
                string hoten = firstname + " " + middle_name + "" + lastname;
                var res = _accountService.CheckLoginFB(username);

                if (res == null)
                {
                    int res2 = _accountService.ThemAccountFB(username, email, hoten);
                    if (res2 == 1)
                    {
                        //cooki
                        var res3 = _accountService.CheckLoginFB(username);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddDays(2), false, username, FormsAuthentication.FormsCookiePath);

                        string encTicket = FormsAuthentication.Encrypt(ticket);

                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                        //session
                        LoginSession.createSession(res3);
                        Session["username"] = res3;
                        return Redirect("/");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { ac = "error" });
                    }
                }
                else
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddDays(2), false, username, FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);

                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    //session
                    LoginSession.createSession(res);
                    Session["username"] = res;
                    return RedirectToAction("Index", "Home");
                }

            }
            return RedirectToAction("Index", "Home", new { ac = "error" });
        }

        //GuiLoiNhan //LoginFB
        [HttpPost]
        public ActionResult GuiLoiNhan(FormCollection c)
        {
            string hoten = c["hoten"].ToString();
            string email = c["email"].ToString();
            string noidong = c["noidong"].ToString();
            _lienHeService.insertLoiNhan(hoten, email, noidong);
            return RedirectToAction("Contact", "Home", new { ac = "loinhan" });
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
            return Redirect("/");
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

            int res = _accountService.ChangePassword(id, passold, passnew, repassnew);
            if (res == 0)
            {
                return RedirectToAction("Account/" + id, "Home", new { ac3 = "error3" });
            }
            else
            {
                return RedirectToAction("Account/" + id, "Home",new { ac3 = "succes" });
            }
        }
     
    }
}