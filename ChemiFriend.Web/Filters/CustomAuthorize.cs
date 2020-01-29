using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChemiFriend.Utility;
using ChemiFriend.Models;
using ChemiFriend.Data.Repository;

namespace ChemiFriend.Web.Filters
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public string Roles { get; set; }
        IUserMasterRepository _userMaster;
        public CustomAuthorize()
        {
            _userMaster = new UserMasterRepository();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserAuthenticate.IsAuthenticated)
            {
                var rols = Roles.Split(',');
                string s = Enum.GetName(typeof(Roles), UserAuthenticate.Role);
                if (rols.Contains(s))
                    return;
            }
            base.OnAuthorization(filterContext);
        }
    }
}