using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity;
using ChemiFriend.Utility;
using ChemiFriend.Data;

namespace ChemiFriend.Models
{
    public class UserAuthenticate
    {
        public static Int64 UserId
        {
            get
            {
                return Convert.ToInt64(GetUserDetailsFromCookie(1));
            }
        }

        public static string UserName
        {
            get
            {
                return GetUserDetailsFromCookie(3);
            }
        }
        public static string Name
        {
            get
            {
                return GetUserDetailsFromCookie(2);

            }
        }
        public static bool IsAuthenticated
        {
            get
            {
                var User = GetUserDetailsFromCookie();
                return User == null ? false : true;
            }
        }
        public static int Role
        {
            get
            {
                return Convert.ToInt32(GetUserDetailsFromCookie(4));
            }
        }
        public static Usermaster GetUser(Int64 UserId)
        {
            IUserMasterRepository _account = new UserMasterRepository(); ;
            //return _account.GetUser(UserId);
            return _account.FindBy(x => x.UserId == UserId).FirstOrDefault();
        }
        public static string GetUserDetailsFromCookie(int index)
        {
            if (HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"] != null)
            {
                //string CookieValue = CommonHelper.Decrypt(HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Value);
                string CookieValue = HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Value;
                if (!string.IsNullOrEmpty(CookieValue))
                {
                    string[] Values = CookieValue.Split('!');
                    if (Values.Length > 2)
                    {
                        if (Values[0] == HttpContext.Current.Session.SessionID)
                        {
                            return Values[index];
                        }
                    }
                }
            }
            return null;
        }
        public static Usermaster GetUserDetailsFromCookie()
        {
            if (HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"] != null)
            {
                //string CookieValue = CommonHelper.Decrypt(HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Value);
                string CookieValue = HttpContext.Current.Request.Cookies["ChemiFriend_UserSession"].Value;
                if (!string.IsNullOrEmpty(CookieValue))
                {
                    string[] Values = CookieValue.Split('!');
                    if (Values.Length > 2)
                    {
                        if (Values[0] == HttpContext.Current.Session.SessionID)
                        {
                            IUserMasterRepository _account = new UserMasterRepository();
                            Int64 UserId = Convert.ToInt64(Values[1]);
                            //return _account.GetUser(UserId);
                            return _account.FindBy(x => x.UserId == UserId).FirstOrDefault();
                        }
                    }
                }
            }
            return null;
        }

        public static void Logout(HttpContext context)
        {
            if (context.Request.Cookies["ChemiFriend_UserSession"] != null)
            {
                context.Response.Cookies["ChemiFriend_UserSession"].Expires = DateTime.Now.AddDays(-1);
            }
        }

    }
}