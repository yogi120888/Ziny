using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Models;
using ChemiFriend.Utility;
using ChemiFriend.Web.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemiFriend.Web.Areas.Admin.Controllers
{
    // [CustomAuthorize(Roles = "Admin,Corporate")]
    public class MasterController : Controller
    {
        IUserMasterRepository _user;
        IcommonRepository _commonRepository;

        #region [Constructor]
        public MasterController()
        {
            _user = new UserMasterRepository();
            _commonRepository = new commonRepository();
        }
        #endregion

        #region [Produc Category API's]



        #endregion

        //---------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && _user != null)
                _user.Dispose();

            base.Dispose(disposing);
        }
    }
}