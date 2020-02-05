using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
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

            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                //if (dealModel.lstSchemes.Count() <= 0)
                //{
                //    ViewBag.Error = "Please add at least 1 scheme";
                //    return View();
                //}

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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDealList()
        {
            return View();
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
