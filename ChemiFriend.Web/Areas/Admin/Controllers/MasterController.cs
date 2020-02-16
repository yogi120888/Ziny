using AutoMapper;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity;
using ChemiFriend.Models;
using ChemiFriend.Utility;
using ChemiFriend.Web.Filters;
using ChemiFriend.Web.Models.InputModel;
using ChemiFriend.Web.Models.Response;
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
        IProductCodeRepository _productCodeRepository;

        #region [Constructor]
        public MasterController()
        {
            _user = new UserMasterRepository();
            _commonRepository = new commonRepository();
            _productCodeRepository = new ProductCodeRepository();
        }
        #endregion

        #region [Produc Code]

        /// <summary>
        /// Create product Code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateProductCode()
        {
            return View();
        }

        /// <summary>
        /// Create Product Code
        /// </summary>
        /// <param name="productCodeModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateProductCode(ProductCodeModel productCodeModel)
        {
            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                try
                {
                    var _getCode = _productCodeRepository.FindBy(x => x.ProductCodes.Trim().ToLower() == productCodeModel.ProductCodes.Trim().ToLower() && x.Status == true).FirstOrDefault();
                    if (_getCode != null)
                    {
                        ViewBag.Error = "Error: Molecules already Exist";
                        return View();
                    }

                    var model = Mapper.Map<ProductCodeModel, ProductCode>(productCodeModel);
                    model.Status = true;
                    _productCodeRepository.Add(model);
                    int rows = _productCodeRepository.SaveChanges();
                    if (rows > 0)
                    {
                        ViewBag.Success = "Molecules created successfully";
                        _response.Type = "success";
                        _response.Message = "Molecules created successfully";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "Something went wrong";
                        _response.Type = "error";
                        _response.Message = "Something went wrong";
                        return View(productCodeModel);
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
        /// Get Produc Category List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProductCodeList()
        {
            var list = _productCodeRepository.FindBy(x => x.Status == true).ToList();

            return View(list);
        }

        /// <summary>
        /// Edit Product Code
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProductCode(Int64 Id)
        {
            var details = _productCodeRepository.FindBy(x => x.ProductCodeId == Id).FirstOrDefault();
            var model = Mapper.Map<ProductCode, ProductCodeModel>(details);

            return View(model);
        }

        /// <summary>
        /// EditProductCode
        /// </summary>
        /// <param name="productCodeModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductCode(ProductCodeModel productCodeModel)
        {
            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                try
                {
                    var _getCode = _productCodeRepository.FindBy (x => x.ProductCodeId != productCodeModel.ProductCodeId && x.ProductCodes.Trim().ToLower() == productCodeModel.ProductCodes.Trim().ToLower() && x.Status == true).FirstOrDefault();
                    if (_getCode != null)
                    {
                        ViewBag.Error = "Error: Molecules already Exist";
                        return View();
                    }

                    var _GetDetails = _productCodeRepository.FindBy(x => x.ProductCodeId == productCodeModel.ProductCodeId).FirstOrDefault();
                    _GetDetails.ProductCodes = productCodeModel.ProductCodes;

                    _productCodeRepository.Update(_GetDetails);
                    int rows = _productCodeRepository.SaveChanges();
                    if (rows > 0)
                    {
                        ViewBag.Success = "Molecules Edited successfully";
                        _response.Type = "success";
                        _response.Message = "Molecules Edited successfully";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "Something went wrong";
                        _response.Type = "error";
                        _response.Message = "Something went wrong";
                        return View(productCodeModel);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error:" + ex.Message;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult DeleteProductCode(Int64 Id)
        {
            ResponseModel _response = new ResponseModel();
            var details = _productCodeRepository.FindBy(x => x.ProductCodeId == Id).FirstOrDefault();
            details.Status = false;
            _productCodeRepository.Update(details);
            int rows = _productCodeRepository.SaveChanges();
            if (rows > 0)
            {
                _response.Type = "success";
                _response.Message = "Molecules deleted successfully";
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
            if (disposing && _user != null)
                _user.Dispose();

            base.Dispose(disposing);
        }
    }
}