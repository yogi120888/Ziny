using AutoMapper;
using ChemiFriend.API.Infrastructure;
using ChemiFriend.API.Models.ImageUploadModel;
using ChemiFriend.API.Models.InputModel;
using ChemiFriend.API.Models.Response;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ChemiFriend.API.Controllers
{
    public class ApiDealController : ApiController
    {
        IDealRepository _dealRepository;
        ISchemeRepository _schemeRepository;

        #region [Construtor]
        public ApiDealController()
        {
            _dealRepository = new DealRepository();
            _schemeRepository = new SchemeRepository();
        }
        #endregion

        #region [Deal API's]

        /// <summary>
        /// Create Deal
        /// </summary>
        /// <param name="dealModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateDeal(DealModel dealModel)
        {
            ResponseModel _response = new ResponseModel();

            var model = Mapper.Map<DealModel, Deal>(dealModel);
            model.CreatedDate = DateTime.Now;
            model.Status = 0;
            model.IsDeleted = false;

            _dealRepository.Add(model);
            int Rows = _dealRepository.SaveChanges();
            if (Rows > 0)
            {
                if (dealModel.lstSchemes != null && dealModel.lstSchemes.Count > 0)
                {
                    // Scheme not mandatory
                    List<Scheme> schemes = new List<Scheme>();
                    foreach (var item in dealModel.lstSchemes)
                    {
                        Scheme objScheme = new Scheme();
                        objScheme.DealId = model.DealId;
                        objScheme.SchemeTypeId = item.SchemeTypeId;
                        objScheme.MinOrderQuantity = item.MinOrderQuantity;
                        objScheme.DealRate = item.DealRate;
                        objScheme.Saving = item.Saving;
                        objScheme.DealScheme = item.DealScheme;
                        objScheme.Status = (int)Status.New;
                        objScheme.IsDeleted = false;
                        objScheme.CreatedBy = item.CreatedBy;
                        objScheme.CreatedDate = DateTime.Now;
                        schemes.Add(objScheme);
                    }
                    _schemeRepository.SaveList(schemes);
                    _schemeRepository.SaveChanges();
                }

                // For Uploading image ----
                if (dealModel.DocProduct != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(dealModel.DocProduct);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        Deal _deal = _dealRepository.FindBy(x => x.DealId == model.DealId).FirstOrDefault();
                        if (_deal != null)
                        {
                            _deal.ProductImagePath = uploadedFileName[0].ToString();
                            _dealRepository.Update(_deal);
                            _dealRepository.SaveChanges();
                        }
                    }
                }

                _response.Type = "success";
                _response.Message = "Scheme added successfully";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Something went wrong";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get Deal List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDealList()
        {
            ResponseWithData<List<GetDealModel>> _response = new ResponseWithData<List<GetDealModel>>();
            var _getList = _dealRepository.GetDealList().ToList();
            if (_getList.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _getList;
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
        /// Get Deal Deails By Id
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDealDeailsById(Int64 dealId)
        {
            ResponseWithData<GetDealModel> _response = new ResponseWithData<GetDealModel>();
            var _details = _dealRepository.GetDealDetailsById(dealId).FirstOrDefault();
            if (_details != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _details;
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
        /// Get Scheme With Deal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetSchemeWithDeal()
        {
            ResponseWithData<List<GetSchemeWithDealModel>> _response = new ResponseWithData<List<GetSchemeWithDealModel>>();
            var _getList = _dealRepository.GetSchemeWithDeal().ToList();
            if (_getList.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _getList;
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

        #region [Scheme API's]

        /// <summary>
        /// Get Scheme List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetSchemeList()
        {
            ResponseWithData<List<GetSchemeModel>> _response = new ResponseWithData<List<GetSchemeModel>>();
            var _getList = _schemeRepository.GetSchemeList().ToList();
            if (_getList.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _getList;
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
        /// Get Scheme List By Deal
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetSchemeListByDeal(Int64 dealId)
        {
            ResponseWithData<List<GetSchemeModel>> _response = new ResponseWithData<List<GetSchemeModel>>();
            var _getList = _schemeRepository.GetSchemeListByDeal(dealId).ToList();
            if (_getList.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _getList;
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
        /// Get Scheme Deails By Id
        /// </summary>
        /// <param name="schemeId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetSchemeDeailsById(Int64 schemeId)
        {
            ResponseWithData<GetSchemeModel> _response = new ResponseWithData<GetSchemeModel>();
            var _details = _schemeRepository.GetSchemeDetailsById(schemeId).FirstOrDefault();
            if (_details != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _details;
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
            if (disposing && _dealRepository != null)
                _dealRepository.Dispose();

            if (disposing && _schemeRepository != null)
                _schemeRepository.Dispose();

            base.Dispose(disposing);
        }

    }
}
