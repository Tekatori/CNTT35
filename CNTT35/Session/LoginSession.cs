using CNTT35.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNTT35.Session
{
    public static class LoginSession
    {
        private static string SessionName = "UserLoginSession";
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

        public static NGUOIDUNG GetSessionInfoLogin()
        {
            NGUOIDUNG nd = new NGUOIDUNG();
            if (HttpContext.Current.Session[SessionName] != null)
            {
                nd = HttpContext.Current.Session[SessionName] as NGUOIDUNG;
            }
            else
                return null;
            return nd;
        }
    }
}