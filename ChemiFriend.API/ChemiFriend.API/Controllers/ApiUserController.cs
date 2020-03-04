using AutoMapper;
using ChemiFriend.API.Models.InputModel;
using ChemiFriend.API.Models.Response;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using ChemiFriend.API.Models.ImageUploadModel;
using ChemiFriend.API.Infrastructure;
using System.IO;
using System.Web;
using ChemiFriend.Utility;
using ChemiFriend.Entity.JsonModel;

namespace ChemiFriend.API.Controllers
{
    // [APIAuthorizeAttribute]
    public class ApiUserController : ApiController
    {
        IcommonRepository _common;
        IUserMasterRepository _userMaster;
        IRegistrationRepository _registrationRepository;
        ILicenceImagesReposiory _licenceImagesReposiory;
        #region [Construtor]
        public ApiUserController()
        {
            _common = new commonRepository();
            _userMaster = new UserMasterRepository();
            _registrationRepository = new RegistrationRepository();
            _licenceImagesReposiory = new LicenceImagesReposiory();
        }
        #endregion

        #region [Public Method]

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="usermaster"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateUser(UsermasterModel usermasterModel)
        {
            ResponseWithData<Usermaster> _response = new ResponseWithData<Usermaster>();
            var _getUser = _userMaster.FindBy(x => x.Phone == usermasterModel.Phone.Trim() || x.Email.Trim().ToLower() == usermasterModel.Email.Trim().ToLower()).FirstOrDefault();
            if (_getUser != null)
            {
                _response.Type = "error";
                _response.Message = "User already Exist";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            var model = Mapper.Map<UsermasterModel, Usermaster>(usermasterModel);
            string[] arrName = model.Email.Split('@');
            model.UserName = model.Email.Trim();
            model.Password = arrName[0] + "@123";
            model.Status = 0;
            model.IsDeleted = false;
            var response = _userMaster.CreateUser(model);
            if (response != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = response;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "error";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Create User Registration
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateRegistration(RegistrationModel registrationModel)
        {
            ResponseModel _response = new ResponseModel();
            var _getUser = _userMaster.FindBy(x => x.UserId == registrationModel.UserId).FirstOrDefault();
            if (_getUser == null)
            {
                _response.Type = "error";
                _response.Message = "User not Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            var _getDetail = _registrationRepository.FindBy(x => x.UserId == registrationModel.UserId).FirstOrDefault();
            if (_getDetail != null)
            {
                _response.Type = "error";
                _response.Message = "Profile already Created";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            var model = Mapper.Map<RegistrationModel, Registration>(registrationModel);
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = registrationModel.CreatedBy;
            model.Status = (int)Status.InActive;
            model.IsDeleted = false;
            var Result = _registrationRepository.CreateUserProfile(model);
            if (Result != null)
            {
                // Update UserMaster Status for Inactive
                Usermaster _details = _userMaster.FindBy(x => x.UserId == Result.UserId).FirstOrDefault();
                if (_details != null)
                {
                    _details.Status = (int)Status.InActive;
                    _userMaster.Update(_details);
                    _userMaster.SaveChanges();
                }
                // For Uploading Licence image ----
                foreach (var item in registrationModel.lstDocLicence)
                {
                    if (item != null)
                    {
                        UploadDocumentModel UploadDocument = new UploadDocumentModel();
                        UploadDocument.documents = new List<DocumentModel>();
                        UploadDocument.ImageSize = 250;
                        UploadDocument.ImageQuality = 250;
                        UploadDocument.documents.Add(item);
                        List<string> uploadedFileName = SavePicture(UploadDocument);
                        if (uploadedFileName.Count > 0)
                        {
                            //Registration _registration = _registrationRepository.FindBy(x => x.RegistrationId == Result.RegistrationId).FirstOrDefault();
                            //if (_registration != null)
                            //{
                            //    _registration.LicenceImage = uploadedFileName[0].ToString();
                            //    _registrationRepository.Update(_registration);
                            //    _registrationRepository.SaveChanges();
                            //}
                            LicenceImages licenceImages = new LicenceImages();
                            licenceImages.ImagePath = uploadedFileName[0].ToString();
                            licenceImages.RegistrationId = Result.RegistrationId;
                            licenceImages.IsActive = true;
                            _licenceImagesReposiory.Add(licenceImages);
                            _licenceImagesReposiory.SaveChanges();
                        }
                    }
                }

                // For Uploading image ----
                if (registrationModel.DocGSTNo != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(registrationModel.DocGSTNo);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        Registration _registration = _registrationRepository.FindBy(x => x.RegistrationId == Result.RegistrationId).FirstOrDefault();
                        if (_registration != null)
                        {
                            _registration.GSTNoImage = uploadedFileName[0].ToString();
                            _registrationRepository.Update(_registration);
                            _registrationRepository.SaveChanges();
                        }
                    }
                }

                _response.Type = "success";
                _response.Message = "Profile created successfully";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "somehing went wrong";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Update User Registred Profile
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateUserProfile(RegistrationModel registrationModel)
        {
            ResponseModel _response = new ResponseModel();
            var _getUser = _userMaster.FindBy(x => x.UserId == registrationModel.UserId).FirstOrDefault();
            Registration _registration = _registrationRepository.FindBy(x => x.RegistrationId == registrationModel.RegistrationId).FirstOrDefault();
            if (_getUser == null)
            {
                _response.Type = "error";
                _response.Message = "User not Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            _registration.FirmName = registrationModel.FirmName;
            _registration.LicenceNo = registrationModel.LicenceNo;
            _registration.FirstName = registrationModel.FirstName;
            _registration.LastName = registrationModel.LastName;
            _registration.PhoneNo = registrationModel.PhoneNo;
            _registration.Email = registrationModel.Email;
            _registration.City = registrationModel.City;
            _registration.ZipCode = registrationModel.ZipCode;
            _registration.Address1 = registrationModel.Address1;
            _registration.Address2 = registrationModel.Address2;
            _registration.PANNo = registrationModel.PANNo;
            _registration.GSTNo = registrationModel.GSTNo;
            _registration.Country = registrationModel.Country;
            _registration.State = registrationModel.State;
            _registration.ModifiedDate = DateTime.Now;
            _registration.ModifiedBy = registrationModel.ModifiedBy;

            var IsResponse = _registrationRepository.UpdateUserProfile(_registration);
            if (IsResponse)
            {
                // For Uploading Licence image ----
                foreach (var item in registrationModel.lstDocLicence)
                {
                    if (registrationModel.lstDocLicence != null)
                    {
                        UploadDocumentModel UploadDocument = new UploadDocumentModel();
                        UploadDocument.documents = new List<DocumentModel>();
                        UploadDocument.ImageSize = 250;
                        UploadDocument.ImageQuality = 250;
                        UploadDocument.documents.Add(item);
                        List<string> uploadedFileName = SavePicture(UploadDocument);
                        if (uploadedFileName.Count > 0)
                        {
                            //if (_registration != null)
                            //{
                            //    // delete exiting images
                            //    string ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + _registration.LicenceImage);
                            //    if (System.IO.File.Exists(ImgPath))
                            //    {
                            //        System.IO.File.Delete(ImgPath);
                            //    }
                            //    string ImgOriginalPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "Original_" + _registration.LicenceImage);
                            //    if (System.IO.File.Exists(ImgOriginalPath))
                            //    {
                            //        System.IO.File.Delete(ImgOriginalPath);
                            //    }
                            //    string TImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "T" + _registration.LicenceImage);
                            //    if (System.IO.File.Exists(TImgPath))
                            //    {
                            //        System.IO.File.Delete(TImgPath);
                            //    }
                            //    //------------------------------

                            //    LicenceImages licenceImages = new LicenceImages();
                            //    licenceImages.ImagePath = uploadedFileName[0].ToString();
                            //    licenceImages.RegistrationId = _registration.RegistrationId;
                            //    licenceImages.IsActive = true;
                            //    _licenceImagesReposiory.Add(licenceImages);
                            //    _licenceImagesReposiory.SaveChanges();
                            //}
                        }
                    }
                }

                // For Uploading image ----
                if (registrationModel.DocGSTNo != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(registrationModel.DocGSTNo);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        if (_registration != null)
                        {
                            // delete exiting images
                            string ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + _registration.GSTNoImage);
                            if (System.IO.File.Exists(ImgPath))
                            {
                                System.IO.File.Delete(ImgPath);
                            }
                            string ImgOriginalPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "Original_" + _registration.GSTNoImage);
                            if (System.IO.File.Exists(ImgOriginalPath))
                            {
                                System.IO.File.Delete(ImgOriginalPath);
                            }
                            string TImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "T" + _registration.GSTNoImage);
                            if (System.IO.File.Exists(TImgPath))
                            {
                                System.IO.File.Delete(TImgPath);
                            }
                            //------------------------------
                            _registration.GSTNoImage = uploadedFileName[0].ToString();
                            _registrationRepository.Update(_registration);
                            _registrationRepository.SaveChanges();
                        }
                    }
                }

                _response.Type = "success";
                _response.Message = "Profile updated successfully";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "somehing went wrong";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get User Detail
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetLoginDetail(string userName, string password)
        {
            ResponseWithData<Usermaster> _response = new ResponseWithData<Usermaster>();
            var _userDetail = _userMaster.FindBy(x => x.UserName.Trim().ToLower() == userName.Trim().ToLower() && x.Password == password).FirstOrDefault();
            if (_userDetail != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _userDetail;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Invalid Credential";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserById(Int64 userId)
        {
            ResponseWithData<Usermaster> _response = new ResponseWithData<Usermaster>();
            var _userDetail = _userMaster.FindBy(x => x.UserId == userId).FirstOrDefault();
            if (_userDetail != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _userDetail;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Invalid Credential";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get Regisration Detail
        /// </summary>
        /// <param name="registerId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetRegisrationDetail(Int64 registerId)
        {
            ResponseWithData<Registration> _response = new ResponseWithData<Registration>();
            var _registrationDetail = _registrationRepository.FindBy(x => x.RegistrationId == registerId).FirstOrDefault();
            if (_registrationDetail != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _registrationDetail;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Invalid Credential";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get Active User List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetActiveUserList()
        {
            ResponseWithData<List<GetUsermasterModel>> _response = new ResponseWithData<List<GetUsermasterModel>>();
            var getList = _userMaster.GetUserList().Where(x => x.Status == (int)Status.Active && x.IsDeleted == false).OrderByDescending(x => x.UserId).ToList();
            if (getList.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = getList;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No record found";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get InActive User List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetInActiveUserList()
        {
            ResponseWithData<List<GetUsermasterModel>> _response = new ResponseWithData<List<GetUsermasterModel>>();
            var getList = _userMaster.GetUserList().Where(x => x.Status != (int)Status.Active && x.IsDeleted == false).OrderByDescending(x => x.UserId).ToList();
            if (getList.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = getList;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No record found";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get Deleted User List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDeletedUserList()
        {
            ResponseWithData<List<GetUsermasterModel>> _response = new ResponseWithData<List<GetUsermasterModel>>();
            var getList = _userMaster.GetUserList().Where(x => x.IsDeleted == true).OrderByDescending(x => x.UserId).ToList();
            if (getList.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = getList;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No record found";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }
        #endregion

        #region [Private Methods]
        private List<string> SavePicture(UploadDocumentModel documents)
        {
            try
            {
                string timeSpan = new TimeSpan(DateTime.UtcNow.Ticks).ToString().Replace(":", "_").Replace(".", "_");
                string diretorycPath = string.Empty;
                string fileName = string.Empty;
                string tFileName = string.Empty;
                int size = Convert.ToInt32(documents.ImageSize);
                int quality = Convert.ToInt32(documents.ImageQuality);

                List<string> generatedIds = new List<string>();
                if (documents.documents.Count > 0)
                {
                    foreach (var item in documents.documents)
                    {
                        if (item.Filebytes.Length > 0)
                        {
                            //var fileGuid = Guid.NewGuid();
                            var fileGuid = Guid.NewGuid().ToString("N").Substring(0, 12);
                            fileName = "Img_" + fileGuid + timeSpan + ".png";
                            tFileName = "TImg_" + fileGuid + timeSpan + ".png";
                            string desti = fileName;

                            diretorycPath = HttpContext.Current.Server.MapPath("~/Images/UploadImg/");
                            if (!Directory.Exists(diretorycPath))
                            {
                                Directory.CreateDirectory(diretorycPath);
                            }

                            item.FileExtenstion = "image/png";
                            item.FileName = fileName;

                            ImageHelper.Resize_Picture(item.Filebytes, fileName, diretorycPath, 0, 0, quality);
                            // It Method used for resizeImage like ttThumbnel
                            ImageHelper.ImageResize(diretorycPath, fileName, tFileName, size, quality);
                            generatedIds.Add(System.Text.RegularExpressions.Regex.Replace(desti, "\\\\", "/"));
                        }
                    }
                }

                return generatedIds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        #endregion

        //---------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && _common != null)
                _userMaster.Dispose();

            base.Dispose(disposing);
        }
    }
}
