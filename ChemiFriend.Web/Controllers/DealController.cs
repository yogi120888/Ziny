using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.Models;
using ChemiFriend.Utility;
using ChemiFriend.Web.Filters;
using ChemiFriend.Web.Models.ImageUploadModel;
using ChemiFriend.Web.Models.InputModel;
using ChemiFriend.Web.Models.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChemiFriend.Web.Controllers
{
    [CustomAuthorize(Roles = "Admin,Distributor")]
    public class DealController : Controller
    {
        string BaseUrl = ConfigurationManager.AppSettings["APIBaseUrl"].ToString();
        IcommonRepository _commonRepository;
        IProductRepository _productRepository;
        IDealRepository _dealRepository;
        ISchemeRepository _schemeRepository;

        #region [Constructor]
        public DealController()
        {
            _commonRepository = new commonRepository();
            _productRepository = new ProductRepository();
            _dealRepository = new DealRepository();
            _schemeRepository = new SchemeRepository();
        }
        #endregion

        #region [DEAL]
        /// <summary>
        /// Create Deal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateDeal()
        {
            ViewBag.BindProduct = _productRepository.BindProducts().ToList();
            ViewBag.BindDealType = _commonRepository.GetDealType().ToList();
            ViewBag.BindDealApplicableFor = _commonRepository.GetDealApplicableFor().ToList();
            // ViewBag.BindProductType = _commonRepository.GetProductType().ToList();
            ViewBag.BindFormType = _commonRepository.GetFormType().ToList();
            ViewBag.BindPackType = _commonRepository.GetPackType().ToList();
            ViewBag.BindGSTApplicable = _commonRepository.GetGSTApplicable().ToList();
            ViewBag.BindSchemeType = _commonRepository.GetSchemeType().ToList();
            IEnumerable<SelectListItem> _applicableTaxType = from ApplicableTaxType att in Enum.GetValues(typeof(ApplicableTaxType))
                                                             select new SelectListItem
                                                             {
                                                                 Text = att.ToString(),
                                                                 Value = Convert.ToInt32(att).ToString()
                                                             };
            ViewBag.ApplicableTaxType = _applicableTaxType;

            return View();
        }

        /// <summary>
        /// Create Deal
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateDeal(DealModel dealModel, HttpPostedFileBase ImageFile)
        {
            ViewBag.BindProduct = _productRepository.BindProducts().ToList();
            ViewBag.BindDealType = _commonRepository.GetDealType().ToList();
            ViewBag.BindDealApplicableFor = _commonRepository.GetDealApplicableFor().ToList();
            // ViewBag.BindProductType = _commonRepository.GetProductType().ToList(); 
            ViewBag.BindFormType = _commonRepository.GetFormType().ToList();
            ViewBag.BindPackType = _commonRepository.GetPackType().ToList();
            ViewBag.BindGSTApplicable = _commonRepository.GetGSTApplicable().ToList();
            ViewBag.BindSchemeType = _commonRepository.GetSchemeType().ToList();
            IEnumerable<SelectListItem> _applicableTaxType = from ApplicableTaxType att in Enum.GetValues(typeof(ApplicableTaxType))
                                                             select new SelectListItem
                                                             {
                                                                 Text = att.ToString(),
                                                                 Value = Convert.ToInt32(att).ToString()
                                                             };
            ViewBag.ApplicableTaxType = _applicableTaxType;

            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                if (dealModel.lstSchemes.Count() <= 0)
                {
                    ViewBag.Error = "Please add at least 1 scheme";
                    return View();
                }
                if (dealModel.lstSchemes != null && dealModel.lstSchemes.Count > 0)
                {
                    foreach (var item in dealModel.lstSchemes)
                    {
                        item.CreatedBy = UserAuthenticate.UserId;
                        item.CreatedDate = DateTime.Now;
                    }
                }

                try
                {
                    // Image byte converter
                    if (ImageFile != null)
                    {
                        dealModel.DocProduct = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            dealModel.DocProduct.Filebytes = imageBytes;
                            dealModel.DocProduct.FileName = ImageFile.FileName;
                            dealModel.DocProduct.FileExtenstion = Path.GetExtension(ImageFile.FileName);
                        }
                    }

                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        dealModel.CreatedBy = UserAuthenticate.UserId;
                        string apiURL = BaseUrl + "api/ApiDeal/CreateDeal";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, dealModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(dealModel);
                            }
                            ViewBag.Success = _response.Message;
                            ModelState.Clear();
                        }
                        else
                        {
                            ViewBag.Error = "Error:" + _response.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error:" + ex.Message;
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            return View();
        }

        /// <summary>
        /// Bind Scheme Type
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BindSchemeType()
        {
            var lstSchemeType = _commonRepository.GetSchemeType().ToList();
            return Json(lstSchemeType, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Product Details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProductDetails(int Id)
        {
            var details = _productRepository.GetProductList().Where(x => x.ProductId == Id).FirstOrDefault();
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Deal List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDealList(int Id)
        {
            List<GetDealModel> models = new List<GetDealModel>();
            string ListStatus = string.Empty;
            if (Id == (int)Status.Active)
            {
                ListStatus = "APPROVED";
                models = _dealRepository.GetDealList().Where(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            }
            else if (Id == (int)Status.Deactive)
            {
                ListStatus = "DEACTIVE";
                models = _dealRepository.GetDealList().Where(x => x.Status == (int)Status.Deactive && x.IsDeleted == false).ToList();
            }
            else
            {
                ListStatus = "UN-APPROVED";
                models = _dealRepository.GetDealList().Where(x => x.Status == (int)Status.Created && x.IsDeleted == false).ToList();
            }
            ViewBag.ListStatus = ListStatus;
            return View(models);
        }

        /// <summary>
        /// Get Scheme List
        /// </summary>
        /// <param name="DealId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchemeList(int DealId)
        {
            var lstScheme = _dealRepository.GetSchemeList().Where(x => x.DealId == DealId && x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            return View(lstScheme);
        }

        /// <summary>
        /// My Deals
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyDeals()
        {
            List<GetDealModel> models = new List<GetDealModel>();
            models = _dealRepository.GetDealList().Where(x => x.CreatedBy == UserAuthenticate.UserId && x.Status == (int)Status.Active && x.IsDeleted == false).ToList();

            return View(models);
        }


        /// <summary>
        /// Get Deal Detail
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDealDetail(Int64 Id)
        {
            var deails = _dealRepository.GetDealList().Where(x => x.DealId == Id).FirstOrDefault();

            return View(deails);
        }

        /// <summary>
        /// Approve Deal
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveDeal(Int64 Id)
        {
            ResponseModel _response = new ResponseModel();
            var details = _dealRepository.FindBy(x => x.DealId == Id).FirstOrDefault();
            details.Status = (int)Status.Active;
            details.ModifiedDate = DateTime.Now;
            details.ModifiedBy = UserAuthenticate.UserId;
            bool IsTrue = _dealRepository.Update(details);
            _dealRepository.SaveChanges();
            if (IsTrue)
            {
                _response.Type = "success";
                _response.Message = "Deal approved successfully";
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Somehing went wrong";
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deacivate Deal
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeacivateDeal(Int64 Id)
        {
            ResponseModel _response = new ResponseModel();
            var details = _dealRepository.FindBy(x => x.DealId == Id).FirstOrDefault();
            details.Status = (int)Status.Deactive;
            details.ModifiedDate = DateTime.Now;
            details.ModifiedBy = UserAuthenticate.UserId;
            bool IsTrue = _dealRepository.Update(details);
            _dealRepository.SaveChanges();
            if (IsTrue)
            {
                _response.Type = "success";
                _response.Message = "Deal deactivated successfully";
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Somehing went wrong";
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //---------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && _dealRepository != null)
                _dealRepository.Dispose();

            if (disposing && _schemeRepository != null)
                _schemeRepository.Dispose();

            if (disposing && _productRepository != null)
                _productRepository.Dispose();

            base.Dispose(disposing);
        }


    }
}
