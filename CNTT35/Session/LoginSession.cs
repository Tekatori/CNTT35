using CNTT35.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNTT35.Session
{
    public class LoginSession
    {
        private static string SessionName = "UserLoginSession";
        public static NGUOIDUNG GetSessionInfoLogin { 
            get { 
                NGUOIDUNG nd = new NGUOIDUNG();
                if(HttpContext.Current.Request.IsAuthenticated)
                {
                    if (HttpContext.Current.Session[SessionName] != null)
                    {
                        nd = HttpContext.Current.Session[SessionName] as NGUOIDUNG;
                    }
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/Home/Index");
                }
                return nd;
            } }
        public static void createSession(NGUOIDUNG nd)
        {
            if (HttpContext.Current.Session[SessionName] == null)
            {
                HttpContext.Current.Session[SessionName] = nd;
            }
        }
        public static void clear()
        {
            HttpContext.Current.Session[SessionName] = null;
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
        }
       
    }
}