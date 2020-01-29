using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemiFriend.Web.Areas.Admin.Controllers
{
    public class CommonController : Controller
    {
        IcommonRepository _commonRepository;
        #region [Constructor]
        public CommonController()
        {
            _commonRepository = new commonRepository();
        }
        #endregion

        #region[Bind Dropdowns]
        //[HttpPost]
        //public ActionResult BindDriverList(int Id)
        //{
        //    int DriverId = _commonRepository.GetAll().Where(x => x.CarId == Id && x.Status == true && x.IsDeleted == false).Select(x => x.DriverId).FirstOrDefault();

        //    return Json(DriverId, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}
