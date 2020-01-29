using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChemiFriend.Web.Helpers;

namespace ChemiFriend.Web.Helpers
{
    public class ManageUser
    {
        public static string GetUserRole()
        {
            if (HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"] != null)
            {
                //return CommonHelper.Decrypt(HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Values["uRole"].ToString());
                return HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Values["uRole"].ToString();
            }
            return "";
        }
        public static string GetUserEmail()
        {
            if (HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"] != null)
            {
                //return CommonHelper.Decrypt(HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Values["uEmail"].ToString());
                return HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Values["uEmail"].ToString();
            }
            return "";
        }

        public static string GetUserId()
        {
            if (HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"] != null)
            {
                //return CommonHelper.Decrypt(HttpContext.Current.Request.Cookies["UserSession"].Values["uID"].ToString());
                return HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Values["uID"].ToString();
            }
            return "";
        }
        public static string GetUserName()
        {
            if (HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"] != null)
            {
                //return CommonHelper.Decrypt(HttpContext.Current.Request.Cookies["UserSession"].Values["uName"].ToString());
                return HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Values["uName"].ToString();
            }
            return "";
        }

        //public static string GetProfilePic()
        //{
        //    if (HttpContext.Current.Request.Cookies["UserSession"] != null)
        //    {
        //        return CommonHelper.Decrypt(HttpContext.Current.Request.Cookies["UserSession"].Values["ProfilePic"].ToString());
        //    }
        //    return "";
        //}
        public static bool IsAuthenticated()
        {
            if (HttpContext.Current.Request.Cookies["UserSession"] != null)
            {
                return true;
            }
            return false;
        }
    }
}