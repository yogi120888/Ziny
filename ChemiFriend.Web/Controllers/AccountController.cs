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
using ChemiFriend.Utility;
using System.Net.Http;
using System.Net.Http.Headers;
using ChemiFriend.Data.IRepository;
using ChemiFriend.ENTITY;
using ChemiFriend.Web.Models.InputModel;
using ChemiFriend.Web.Models.Response;
using System.Configuration;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;
using ChemiFriend.Web.Filters;
using ChemiFriend.Web.Models.ImageUploadModel;
using System.IO;
using AutoMapper;
using ChemiFriend.Entity.JsonModel;

namespace ChemiFriend.Web.Controllers
{
    [CustomAuthorize(Roles = "Admin,Distributor,Retrailer")]
    public class AccountController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        string BaseUrl = ConfigurationManager.AppSettings["APIBaseUrl"].ToString();
        IUserMasterRepository _user;
        IcommonRepository _commonRepository;
        IRegistrationRepository _registrationRepository;
        IDealRepository _dealRepository;
        ISchemeRepository _schemeRepository;
        IProductCategoryRepository _productCategoryRepository;
        IProductSubCategoryRepository _productSubCategoryRepository;
        IProductRepository _productRepository;
        ILicenceImagesReposiory _licenceImagesReposiory;

        public AccountController()
        {
            _user = new UserMasterRepository();
            _commonRepository = new commonRepository();
            _registrationRepository = new RegistrationRepository();
            _dealRepository = new DealRepository();
            _schemeRepository = new SchemeRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productSubCategoryRepository = new ProductSubCategoryRepository();
            _productRepository = new ProductRepository();
            _licenceImagesReposiory = new LicenceImagesReposiory();
        }

        #region[Public Method]

        /// <summary>
        /// Home page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            if (UserAuthenticate.IsAuthenticated)
            {
                int _role = UserAuthenticate.Role;
                if (_role == (int)Roles.Admin)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                //return RedirectToAction("Index", "Account");
            }

            SearchDealModel model = new SearchDealModel();
            model.PageIndex = model.PageIndex == 0 ? 1 : 1;
            var lstPazeSize = new List<SelectListItem>
            {
                 new SelectListItem{ Text="8", Value = "8" },
                 new SelectListItem{ Text="12", Value = "12" },
                 new SelectListItem{ Text="16", Value = "16" },
                 new SelectListItem{ Text="20", Value = "20" },
            };

            var lstDealType = new List<SelectListItem>
            {
                 new SelectListItem{ Text="All Deals", Value = "1" },
                 new SelectListItem{ Text="Trending Deals", Value = "2" },
                 new SelectListItem{ Text="Regular Deals", Value = "3" },
                 new SelectListItem{ Text="Local Deals", Value = "4" },
                 new SelectListItem{ Text="Bulk deals", Value = "5" },
                 new SelectListItem{ Text="Regular Products", Value = "6" }
            };

            var lstSorting = new List<SelectListItem>
            {
                 new SelectListItem{ Text="Recent Deals", Value = "1" },
                 new SelectListItem{ Text="Closing Soon", Value = "2" },
                 new SelectListItem{ Text="Closing Late", Value = "3" },
                 new SelectListItem{ Text="Product Name A to Z", Value = "4" },
                 new SelectListItem{ Text="Product Name Z to A", Value = "5" },
                 new SelectListItem{ Text="Rate Low to High", Value = "6" },
                 new SelectListItem{ Text="Rate High to Low", Value = "7" }
            };

            ViewBag.BindPageSize = new SelectList(lstPazeSize, "Value", "Text", model.PageSize);
            ViewBag.BindDealType = new SelectList(lstDealType, "Value", "Text", model.DealType);
            ViewBag.BindSorting = new SelectList(lstSorting, "Value", "Text", model.Sort);

            return View(model);
        }

        /// <summary>
        /// Load Products
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult _LoadProduct(SearchDealModel model)
        {
            model.PageIndex = model.PageIndex == 0 ? 1 : model.PageIndex;
            var query = _dealRepository.GetDealList();
            if (!string.IsNullOrEmpty(model.DealType))
            {

            }
            if (!string.IsNullOrEmpty(model.Sort))
            {

            }
            if (!string.IsNullOrEmpty(model.Search))
            {
                query = query.Where(x => x.ProductName.Contains(model.Search));
            }

            model.TotalRecordCount = query.Count();
            var _getList = query.OrderBy(a => a.DealId).Skip(((model.PageIndex - 1) * model.PageSize)).Take(model.PageSize).ToList();
            int pageCount = (model.TotalRecordCount / model.PageSize) + ((model.TotalRecordCount % model.PageSize) > 0 ? 1 : 0);
            model.TotalPageCount = pageCount;
            // bind schemes
            foreach (var item in _getList)
            {
                item.lstSchemes = _schemeRepository.FindBy(x => x.DealId == item.DealId).ToList();
            }
            model.getDealModels = _getList;

            return PartialView(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddToCart(AddToCartModel model)
        {
            try
            {

                if (Session["cart"] == null)
                {
                    List<AddToCartModel> li = new List<AddToCartModel>();
                    li.Add(model);
                    Session["cart"] = li;
                    Session["count"] = 1;
                }
                else
                {
                    List<AddToCartModel> li = (List<AddToCartModel>)Session["cart"];
                    //List<Mobiles> li = (List<Mobiles>)Session["cart"];
                    li.Add(model);
                    Session["cart"] = li;
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                int count = Convert.ToInt32(Session["count"]);
                return Json(new { Type = "success", Count = count, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Type = "error", Count = 0, }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// My order list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyOrder()
        {
            List<OrderDetailModel> objModel = new List<OrderDetailModel>();
            if (Session["cart"] != null)
            {
                List<AddToCartModel> list = new List<AddToCartModel>();
                list = (List<AddToCartModel>)Session["cart"];
                foreach (var item in list)
                {
                    var _orderDetail = _dealRepository.GetOrderDetail().Where(x => x.ProductId == item.ProductId && x.DealId == item.DealId && x.SchemeId == item.SchemeId).FirstOrDefault();
                    if (_orderDetail != null)
                    {
                        _orderDetail.UserId = item.UserId;
                        _orderDetail.Quantity = item.Quantity;
                    }
                    objModel.Add(_orderDetail);
                }
            }
            return View(objModel);
        }

        /// <summary>
        /// Remove Item from cart list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RemoveItem(Int64 DealId)
        {
            List<AddToCartModel> li = (List<AddToCartModel>)Session["cart"];
            li.RemoveAll(x => x.DealId == DealId);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return RedirectToAction("MyOrder", "Account");
        }


        /// <summary>
        /// LIST OF COMPANIES
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult _GetCategoryList()
        {
            var CategoryList = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            return View(CategoryList);
        }

        /// <summary>
        /// LIST OF BRAND
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetSubCategoryList(Int64 Id)
        {
            ViewBag.Category = Convert.ToString(_productCategoryRepository.FindBy(x => x.ProductCategoryId == Id).Select(x => x.ProductCategoryName).FirstOrDefault()).ToUpper();
            SearchSubCategoryModel model = new SearchSubCategoryModel();
            model.CategoryId = Id;
            model.getSubCategoryModel = _productSubCategoryRepository.GetProductSubCategoryList().Where(x => x.ProductCategoryId == Id && x.Status == (int)Status.Active && x.IsDeleted == false).ToList();

            return View(model);
        }

        /// <summary>
        /// LIST OF BRAND
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult _GetSubCategoryList(SearchSubCategoryModel model)
        {
            var query = _productSubCategoryRepository.GetProductSubCategoryList().Where(x => x.ProductCategoryId == model.CategoryId && x.Status == (int)Status.Active && x.IsDeleted == false);
            if (!string.IsNullOrEmpty(model.Search))
            {
                query = query.Where(x => x.ProductSubCategoryName.Contains(model.Search));
            }
            model.getSubCategoryModel = query.ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Get Product List
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetProductList(Int64 Id)
        {
            ViewBag.SubCategory = Convert.ToString(_productSubCategoryRepository.FindBy(x => x.ProductSubCategoryId == Id).Select(x => x.ProductSubCategoryName).FirstOrDefault()).ToUpper();
            SearchProductModel model = new SearchProductModel();
            model.SubCategoryId = Id;
            model.getProductModel = _productRepository.GetProductList().Where(x => x.ProductSubCategoryId == Id && x.Status == (int)Status.Active && x.IsDeleted == false).ToList();

            return View(model);
        }

        /// <summary>
        /// List of Products
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult _GetProductList(SearchProductModel model)
        {
            var query = _productRepository.GetProductList().Where(x => x.ProductSubCategoryId == model.SubCategoryId && x.Status == (int)Status.Active && x.IsDeleted == false);
            if (!string.IsNullOrEmpty(model.Search))
            {
                query = query.Where(x => x.ProductName.Contains(model.Search));
            }
            model.getProductModel = query.ToList();

            return PartialView(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult _NewOffers()
        {
            return View();
        }

        /// <summary>
        /// Get Login
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult login()
        {
            if (UserAuthenticate.IsAuthenticated)
                return RedirectToAction("Dashboard", "Admin");
            return View();
        }

        /// <summary>
        /// Get Login
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel Model)
        {
            if (ModelState.IsValid)
            {
                Usermaster user = _user.DoLogin(Model.Username, Model.Password);
                if (user != null)
                {
                    TempData["UserId"] = user.UserId;
                    AddLoginCookie(user.UserId, user.Name, user.UserName, user.Role, "Token");
                    // Using for new registration
                    var registrationDetails = _registrationRepository.FindBy(x => x.UserId == user.UserId).FirstOrDefault();
                    if (registrationDetails != null && user.Role == (int)Roles.Admin && user.Status == (int)Status.Active)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else if (registrationDetails != null && user.Role == (int)Roles.Distributor && user.Status == (int)Status.Active)
                    {
                        return RedirectToAction("Index", "Account");
                    }
                    else if (registrationDetails != null && user.Role == (int)Roles.Retrailer && user.Status == (int)Status.Active)
                    {
                        return RedirectToAction("Index", "Account");
                    }
                    else if (registrationDetails != null && user.Status == (int)Status.InActive)
                    {
                        return RedirectToAction("RequestForApproving", "Account");
                    }
                    else if (registrationDetails == null && user.Status == (int)Status.Created)
                    {
                        return RedirectToAction("CreateRegistration", "Account");
                    }
                    else
                    {
                        return RedirectToAction("CreateRegistration", "Account");
                    }
                }
                else
                {
                    ViewBag.Message = "The Username Or Password you enterd is invalid";
                }
            }
            return View();
        }

        /// <summary>
        /// Request For Approving
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestForApproving()
        {
            Int64 userId = Convert.ToInt64(TempData["UserId"]);
            var userDetails = _user.FindBy(x => x.UserId == userId).FirstOrDefault();
            return View(userDetails);
        }

        /// <summary>
        /// Get Active User List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetActiveUserList()
        {
            return View();
        }

        /// <summary>
        /// Get InActive User List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInActiveUserList()
        {
            return View();
        }

        /// <summary>
        /// Get Deleted User List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDeletedUserList()
        {
            return View();
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewBag.Role = _commonRepository.GetUserRoleList().ToList();
            return View();
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="usermasterModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateUser(UsermasterModel usermasterModel)
        {
            ViewBag.Role = _commonRepository.GetUserRoleList().ToList();
            ResponseWithData<Usermaster> _response = new ResponseWithData<Usermaster>();
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        string apiURL = BaseUrl + "api/ApiUser/CreateUser";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, usermasterModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseWithData<Usermaster>>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(usermasterModel);
                            }
                            ViewBag.UserName = _response.data.UserName;
                            ViewBag.Password = _response.data.Password;
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
            return View();
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUser(Int64 Id)
        {
            ResponseModel _response = new ResponseModel();
            var details = _user.FindBy(x => x.UserId == Id).FirstOrDefault();
            if (details != null)
            {
                details.IsDeleted = true;
                _user.Update(details);
                int rows = _user.SaveChanges();
                if (rows > 0)
                {
                    _response.Type = "success";
                    _response.Message = "User deleted successfully";
                }
                else
                {
                    _response.Type = "error";
                    _response.Message = "Somehing went wrong";
                }
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create Registration
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateRegistration()
        {
            Int64 userId = Convert.ToInt64(TempData["UserId"]);
            ViewBag.BindState = _commonRepository.GetState(1).ToList();
            var userDetails = _user.FindBy(x => x.UserId == userId).FirstOrDefault();
            RegistrationModel registrationModel = new RegistrationModel();
            registrationModel.UserId = userId;
            if (userDetails != null)
            {
                registrationModel.FirstName = userDetails.Name;
                registrationModel.PhoneNo = userDetails.Phone;
                registrationModel.Email = userDetails.Email;
            }
            return View(registrationModel);
        }

        /// <summary>
        /// Create Registration
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateRegistration(RegistrationModel registrationModel, HttpPostedFileBase[] LicenceImage, HttpPostedFileBase GSTNoImage)
        {
            ViewBag.BindState = _commonRepository.GetState(1).ToList();
            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                try
                {
                    // Image byte converter
                    if (LicenceImage != null && LicenceImage.Count() > 0)
                    {
                        List<DocumentModel> _lstLicence = new List<DocumentModel>();
                        foreach (var item in LicenceImage)
                        {
                            DocumentModel _Licence = new DocumentModel();
                            using (var memoryStream = new MemoryStream())
                            {
                                item.InputStream.CopyTo(memoryStream);
                                byte[] imageBytes = memoryStream.ToArray();
                                _Licence.Filebytes = imageBytes;
                                _Licence.FileName = item.FileName;
                                _Licence.FileExtenstion = Path.GetExtension(item.FileName);
                            }
                            _lstLicence.Add(_Licence);
                        }
                        registrationModel.lstDocLicence = _lstLicence;
                    }
                    // Image byte converter
                    if (GSTNoImage != null)
                    {
                        registrationModel.DocGSTNo = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            GSTNoImage.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            registrationModel.DocGSTNo.Filebytes = imageBytes;
                            registrationModel.DocGSTNo.FileName = GSTNoImage.FileName;
                            registrationModel.DocGSTNo.FileExtenstion = Path.GetExtension(GSTNoImage.FileName);
                        }
                    }

                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        registrationModel.CreatedBy = UserAuthenticate.UserId;
                        string apiURL = BaseUrl + "api/ApiUser/CreateRegistration";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, registrationModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(registrationModel);
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

            return View();
        }

        /// <summary>
        /// View Registration
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewRegistration(Int64 Id)
        {
            ViewBag.BindState = _commonRepository.GetState(1).ToList();
            RegistrationModel registrationModel = new RegistrationModel();
            Registration registration = _registrationRepository.FindBy(x => x.UserId == Id).FirstOrDefault();
            var model = Mapper.Map<Registration, RegistrationModel>(registration);
            // Get imaglistes 
            ViewBag.ImagesList = _licenceImagesReposiory.FindBy(x => x.RegistrationId == registration.RegistrationId && x.IsActive == true).ToList();
            return View(model);
        }

        /// <summary>
        /// Edit Registration
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRegistration(Int64 Id)
        {
            ViewBag.BindState = _commonRepository.GetState(1).ToList();
            RegistrationModel registrationModel = new RegistrationModel();
            Registration registration = _registrationRepository.FindBy(x => x.UserId == Id).FirstOrDefault();
            var model = Mapper.Map<Registration, RegistrationModel>(registration);

            return View(model);
        }

        /// <summary>
        /// Edit Registration
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <param name="LicenceImage"></param>
        /// <param name="GSTNoImage"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRegistration(RegistrationModel registrationModel, HttpPostedFileBase[] LicenceImage, HttpPostedFileBase GSTNoImage)
        {
            ViewBag.BindState = _commonRepository.GetState(1).ToList();
            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                try
                {
                    // Image byte converter
                    if (LicenceImage != null && LicenceImage.Count() > 0)
                    {
                        List<DocumentModel> _lstLicence = new List<DocumentModel>();
                        foreach (var item in LicenceImage)
                        {
                            DocumentModel _Licence = new DocumentModel();
                            using (var memoryStream = new MemoryStream())
                            {
                                item.InputStream.CopyTo(memoryStream);
                                byte[] imageBytes = memoryStream.ToArray();
                                _Licence.Filebytes = imageBytes;
                                _Licence.FileName = item.FileName;
                                _Licence.FileExtenstion = Path.GetExtension(item.FileName);
                            }
                            _lstLicence.Add(_Licence);
                        }
                        registrationModel.lstDocLicence = _lstLicence;
                    }

                    // Image byte converter
                    if (GSTNoImage != null)
                    {
                        registrationModel.DocGSTNo = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            GSTNoImage.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            registrationModel.DocGSTNo.Filebytes = imageBytes;
                            registrationModel.DocGSTNo.FileName = GSTNoImage.FileName;
                            registrationModel.DocGSTNo.FileExtenstion = Path.GetExtension(GSTNoImage.FileName);
                        }
                    }

                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        registrationModel.ModifiedBy = UserAuthenticate.UserId;
                        registrationModel.ModifiedDate = DateTime.Now;

                        string apiURL = BaseUrl + "api/ApiUser/UpdateUserProfile";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, registrationModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(registrationModel);
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

            return View();
        }

        /// <summary>
        /// Approve User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveUser(Int64 Id)
        {
            ResponseModel _response = new ResponseModel();
            var details = _user.FindBy(x => x.UserId == Id).FirstOrDefault();
            if (details != null)
            {
                details.Status = (int)Status.Active;
                _user.Update(details);
                int rows = _user.SaveChanges();
                if (rows > 0)
                {
                    var _registration = _registrationRepository.FindBy(x => x.UserId == Id).FirstOrDefault();
                    if (_registration != null)
                    {
                        _registration.Status = (int)Status.Active;
                        _registrationRepository.Update(_registration);
                        _user.SaveChanges();
                    }

                    ViewBag.Success = "User deleted successfully";
                    _response.Type = "success";
                    _response.Message = "User deleted successfully";
                }
                else
                {
                    ViewBag.Error = "Somehing went wrong !";
                    _response.Type = "error";
                    _response.Message = "Somehing went wrong";
                }
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }


        //[AllowAnonymous]
        //public ActionResult ResetPassword()
        //{
        //    var model = new ResetPasswordModel();
        //    return View(model);
        //}

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ResetPassword(string Email)
        {
            var users = _user.FindBy(c => c.Email.ToLower() == Email.ToLower()).FirstOrDefault();
            if (users != null)
            {
                bool sucess = true;
                //  bool sucess = _messageService.PasswordResetLinkEmail(users, confirmationToken);
                if (sucess)
                {
                    return Json(new
                    {
                        sucess = true,
                        msg = "To complete the password process look for an email in your inbox that provides further instructions."
                    });
                }
                else
                {
                    return Json(new
                    {
                        sucess = false,
                        msg = "Sorry ! Password confirmation Mail is Failed"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    sucess = false,
                    msg = "This email address " + Email + " does not exist"
                });
            }
        }

        /// <summary>
        /// Log Out
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            HttpContext context = System.Web.HttpContext.Current;
            UserAuthenticate.Logout(context);
            return RedirectToAction("Index", "Account");
        }

        #endregion

        #region[Private Method]
        private void AddLoginCookie(Int64 UserId, string Name, string UserName, int Role, string Token)
        {
            var cookie = new HttpCookie("ChemiFriend_UserSession");
            // cookie.Value = UserId.ToString();
            //cookie.Value = CommonHelper.Encrypt(Session.SessionID + "!" + UserId.ToString() + "!" + Role + "!" + Name + "!" + UserName + "!" + Token);
            //cookie.Value = Session.SessionID + "!" + UserId.ToString() + "!" + UserName + "!" + UserName + "!" + Role;
            //cookie.Values["uID"] = CommonHelper.Encrypt(UserId.ToString());
            //cookie.Values["uEmail"] = CommonHelper.Encrypt(UserName.ToString());
            //cookie.Values["uName"] = CommonHelper.Encrypt(UserName.ToString());
            //cookie.Values["uRole"] = CommonHelper.Encrypt(Role.ToString());


            cookie.Value = Session.SessionID + "!" + UserId.ToString() + "!" + Name + "!" + UserName + "!" + Role + "!";
            cookie.Values["uID"] = UserId.ToString();
            cookie.Values["uName"] = UserName.ToString();
            cookie.Values["uEmail"] = UserName.ToString();
            cookie.Values["uRole"] = Role.ToString();
            cookie.Expires = DateTime.Now.AddHours(24);
            Response.Cookies.Add(cookie);

            var tokencookie = new HttpCookie("Token");
            DateTime ExpiresOn = DateTime.Now.AddHours(24);
            tokencookie.Value = CommonHelper.Encrypt(UserId + "|" + ExpiresOn.ToString());
            tokencookie.Value = Token; //CommonHelper.Encrypt(UserId + "|" + ExpiresOn.ToString());
            tokencookie.Expires = DateTime.Now.AddHours(20);
            Response.Cookies.Add(tokencookie);
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing && _user != null)
                _user.Dispose();
            base.Dispose(disposing);
        }
    }
}