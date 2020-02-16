using log4net;
using ChemiFriend.Data.DataContext;
using ChemiFriend.Data;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChemiFriend.Models;

namespace ChemiFriend.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        IGenericRepository<Usermaster> _user;

        public HomeController()
        {
            _user = new UserMasterRepository();
        }

        public ActionResult Index()
        {
            if (UserAuthenticate.IsAuthenticated)
                return RedirectToAction("Dashboard", "Admin");

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _user != null)
                _user.Dispose();

            base.Dispose(disposing);
        }
    }
}