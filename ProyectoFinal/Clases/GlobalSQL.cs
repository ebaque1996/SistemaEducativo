using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;

namespace ProyectoFinal.Clases
{
    public class GlobalSQL
    {
        public static void WriteCookie(string name, string value)
        {
            var cookie = new HttpCookie(name, value);
            HttpContext.Current.Response.Cookies.Set(cookie);
            //System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
        }

    }
}