using ChemiFriend.Data.Repository;
using ChemiFriend.Models;
using ChemiFriend.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemiFriend.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        IUserMasterRepository _user;
        public DashboardController()
        {
            _user = new UserMasterRepository();
        }
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            Int64 userId = UserAuthenticate.UserId;
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassWord model)
        {
            if (ModelState.IsValid)
            {
                //var user = _user.GetUser(UserAuthenticate.UserId);
                var user = _user.FindBy(x => x.UserId == UserAuthenticate.UserId).FirstOrDefault();
                if (user != null)
                {
                    if (user.Password == model.OldPassword)
                    {
                        user.Password = model.NewPassword;
                        bool Success = _user.Update(user);
                        ViewBag.Message = Success ? "Password has been changed." : "The Login Id Or Password You enterd Is Invalid";
                    }
                    else
                    {
                        ModelState.AddModelError("OldPassword", "Please enter correct current password.");
                    }
                }
            }
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _user != null)
                _user.Dispose();

            base.Dispose(disposing);
        }

    }
}