using AutoMapper;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.ENTITY;
using ChemiFriend.Models;
using ChemiFriend.Utility;
using ChemiFriend.Web.Filters;
using ChemiFriend.Web.Models.ImageUploadModel;
using ChemiFriend.Web.Models.InputModel;
using ChemiFriend.Web.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChemiFriend.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        string BaseUrl = ConfigurationManager.AppSettings["APIBaseUrl"].ToString();
        IcommonRepository _commonRepository;
        IProductCategoryRepository _productCategoryRepository;
        IProductSubCategoryRepository _productSubCategoryRepository;
        IProductRepository _productRepository;

        #region [Constructor]
        public ProductController()
        {
            _commonRepository = new commonRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productSubCategoryRepository = new ProductSubCategoryRepository();
            _productRepository = new ProductRepository();
        }
        #endregion

        #region [Produc Category]

        /// <summary>
        /// Create product category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateProductCategory()
        {
            // return View("~/Areas/Admin/Views/Product/CreateProductCategory.cshtml");
            return View();
        }

        /// <summary>
        /// Create Product category
        /// </summary>
        /// <param name="productCategoryModel"></param>
        /// <param name="ImageFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateProductCategory(ProductCategoryModel productCategoryModel, HttpPostedFileBase ImageFile)
        {
            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                if (ImageFile == null)
                {
                    ViewBag.Error = "Company Image is required.";
                    return View();
                }

                var _getCategory = _productCategoryRepository.FindBy(x => x.ProductCategoryName.Trim().ToLower() == productCategoryModel.ProductCategoryName.Trim().ToLower() && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
                if (_getCategory != null)
                {
                    ViewBag.Error = "Error: Company already Exist";
                    return View();
                }

                try
                {
                    // Image byte converter
                    if (ImageFile != null)
                    {
                        productCategoryModel.DocProductCategory = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            productCategoryModel.DocProductCategory.Filebytes = imageBytes;
                            productCategoryModel.DocProductCategory.FileName = ImageFile.FileName;
                            productCategoryModel.DocProductCategory.FileExtenstion = Path.GetExtension(ImageFile.FileName);
                        }
                    }
                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        productCategoryModel.CreatedBy = UserAuthenticate.UserId;
                        string apiURL = BaseUrl + "api/ApiProduct/CreateProductCategory";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, productCategoryModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(productCategoryModel);
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
        /// Get Produc Category List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProductCategoryList()
        {
            var list = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();

            //return View("~/Areas/Admin/Views/Product/GetProductCategoryList.cshtml", list);
            return View(list);
        }

        /// <summary>
        /// Edit Product Category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProductCategory(Int64 Id)
        {
            var details = _productCategoryRepository.FindBy(x => x.ProductCategoryId == Id).FirstOrDefault();
            var model = Mapper.Map<ProductCategory, ProductCategoryModel>(details);

            return View(model);
        }
        /// <summary>
        /// Edit Product Category
        /// </summary>
        /// <param name="productCategoryModel"></param>
        /// <param name="ImageFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductCategory(ProductCategoryModel productCategoryModel, HttpPostedFileBase ImageFile)
        {
            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                try
                {
                    // Image byte converter
                    if (ImageFile != null)
                    {
                        productCategoryModel.DocProductCategory = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            productCategoryModel.DocProductCategory.Filebytes = imageBytes;
                            productCategoryModel.DocProductCategory.FileName = ImageFile.FileName;
                            productCategoryModel.DocProductCategory.FileExtenstion = Path.GetExtension(ImageFile.FileName);
                        }
                    }
                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        productCategoryModel.ModifiedBy = UserAuthenticate.UserId;
                        productCategoryModel.ModifiedDate = DateTime.Now;
                        string apiURL = BaseUrl + "api/ApiProduct/EditProductCategory";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, productCategoryModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(productCategoryModel);
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
        /// Delete Product Category
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProductCategory(Int64 Id)
        {
            ResponseModel _response = new ResponseModel();
            var details = _productCategoryRepository.FindBy(x => x.ProductCategoryId == Id).FirstOrDefault();
            details.IsDeleted = true;
            details.ModifiedDate = DateTime.Now;
            details.ModifiedBy = UserAuthenticate.UserId;
            bool IsTrue = _productCategoryRepository.EditProductCategory(details);
            if (IsTrue)
            {
                _response.Type = "success";
                _response.Message = "Company deleted successfully";
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Somehing went wrong";
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region [Produc Sub Category]
        /// <summary>
        /// Create Product sub Category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateProductSubCategory()
        {
            ViewBag.BindProductCategory = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            return View();
        }

        /// <summary>
        /// Create Product Sub Category
        /// </summary>
        /// <param name="productCategoryModel"></param>
        /// <param name="ImageFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateProductSubCategory(ProductSubCategoryModel productSubCategoryModel, HttpPostedFileBase ImageFile)
        {
            ViewBag.BindProductCategory = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                //if (ImageFile == null)
                //{
                //    ViewBag.Error = "Brand Image is required.";
                //    return View();
                //}

                try
                {
                    // Image byte converter
                    if (ImageFile != null)
                    {
                        productSubCategoryModel.DocProductSubCategory = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            productSubCategoryModel.DocProductSubCategory.Filebytes = imageBytes;
                            productSubCategoryModel.DocProductSubCategory.FileName = ImageFile.FileName;
                            productSubCategoryModel.DocProductSubCategory.FileExtenstion = Path.GetExtension(ImageFile.FileName);
                        }
                    }
                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        productSubCategoryModel.CreatedBy = UserAuthenticate.UserId;
                        string apiURL = BaseUrl + "api/ApiProduct/CreateProductSubCategory";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, productSubCategoryModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(productSubCategoryModel);
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
        /// Edit Product Sub Category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProductSubCategory(Int64 Id)
        {
            ViewBag.BindProductCategory = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            var details = _productSubCategoryRepository.FindBy(x => x.ProductSubCategoryId == Id).FirstOrDefault();
            var model = Mapper.Map<ProductSubCategory, ProductSubCategoryModel>(details);

            return View(model);
        }

        /// <summary>
        /// Edit Product Sub Category
        /// </summary>
        /// <param name="productSubCategoryModel"></param>
        /// <param name="ImageFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductSubCategory(ProductSubCategoryModel productSubCategoryModel, HttpPostedFileBase ImageFile)
        {
            ViewBag.BindProductCategory = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                try
                {
                    // Image byte converter
                    if (ImageFile != null)
                    {
                        productSubCategoryModel.DocProductSubCategory = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            productSubCategoryModel.DocProductSubCategory.Filebytes = imageBytes;
                            productSubCategoryModel.DocProductSubCategory.FileName = ImageFile.FileName;
                            productSubCategoryModel.DocProductSubCategory.FileExtenstion = Path.GetExtension(ImageFile.FileName);
                        }
                    }
                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        productSubCategoryModel.ModifiedBy = UserAuthenticate.UserId;
                        productSubCategoryModel.ModifiedDate = DateTime.Now;
                        string apiURL = BaseUrl + "api/ApiProduct/EditProductSubCategory";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, productSubCategoryModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(productSubCategoryModel);
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
        /// Get Product Sub Category List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProductSubCategoryList()
        {
            var list = _productSubCategoryRepository.GetProductSubCategoryList().Where(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            return View(list);
        }

        /// <summary>
        /// Delete Product Sub Category
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProductSubCategory(Int64 Id)
        {
            ResponseModel _response = new ResponseModel();
            var details = _productSubCategoryRepository.FindBy(x => x.ProductSubCategoryId == Id).FirstOrDefault();
            details.IsDeleted = true;
            details.ModifiedDate = DateTime.Now;
            details.ModifiedBy = UserAuthenticate.UserId;
            bool IsTrue = _productSubCategoryRepository.EditProductSubCategory(details);
            if (IsTrue)
            {
                _response.Type = "success";
                _response.Message = "Brand deleted successfully";
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Somehing went wrong";
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [Produc]

        /// <summary>
        /// Get Produc List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProductList()
        {
            ResponseWithData<List<GetProductModel>> _response = new ResponseWithData<List<GetProductModel>>();
            var _getList = _productRepository.GetProductList().Where(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();

            return View(_getList);
        }

        /// <summary>
        /// Create Product 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateProduct()
        {
            ViewBag.BindProductCategory = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ViewBag.BindProductSubCategory = _productSubCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ViewBag.BindProductTypes = _commonRepository.GetProductType().ToList();
            ViewBag.BindProductCode = _commonRepository.GetProductCode().ToList();

            return View();
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="productModel"></param>
        /// <param name="ImageFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateProduct(ProductModel productModel, HttpPostedFileBase ImageFile)
        {
            ViewBag.BindProductCategory = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ViewBag.BindProductSubCategory = _productSubCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ViewBag.BindProductTypes = _commonRepository.GetProductType().ToList();
            ViewBag.BindProductCode = _commonRepository.GetProductCode().ToList();

            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                //if (ImageFile == null)
                //{
                //    ViewBag.Error = "Product Image is required.";
                //    return View();
                //}
                Int64 productSubCategoryId = productModel.ProductSubCategoryId == null ? 0 : productModel.ProductSubCategoryId.Value;
                Product _product = _productRepository.FindBy(x => x.ProductName.Trim().ToLower() == productModel.ProductName.Trim().ToLower() && x.ProductCategoryId == productModel.ProductCategoryId && x.ProductSubCategoryId == productSubCategoryId && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
                if (_product != null)
                {
                    ViewBag.Error = "Error: Product already exist";
                    return View();
                }

                try
                {
                    // Image byte converter
                    if (ImageFile != null)
                    {
                        productModel.DocProduct = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            productModel.DocProduct.Filebytes = imageBytes;
                            productModel.DocProduct.FileName = ImageFile.FileName;
                            productModel.DocProduct.FileExtenstion = Path.GetExtension(ImageFile.FileName);
                        }
                    }
                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        productModel.CreatedBy = UserAuthenticate.UserId;
                        string apiURL = BaseUrl + "api/ApiProduct/CreateProduct";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, productModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(productModel);
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
        /// Edit Product
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProduct(Int64 Id)
        {
            ViewBag.BindProductCategory = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ViewBag.BindProductSubCategory = _productSubCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ViewBag.BindProductTypes = _commonRepository.GetProductType().ToList();
            ViewBag.BindProductCode = _commonRepository.GetProductCode().ToList();

            var details = _productRepository.FindBy(x => x.ProductId == Id).FirstOrDefault();
            var model = Mapper.Map<Product, ProductModel>(details);
            return View(model);
        }

        /// <summary>
        /// Edit Product
        /// </summary>
        /// <param name="productModel"></param>
        /// <param name="ImageFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProduct(ProductModel productModel, HttpPostedFileBase ImageFile)
        {
            ViewBag.BindProductCategory = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ViewBag.BindProductSubCategory = _productSubCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
            ViewBag.BindProductTypes = _commonRepository.GetProductType().ToList();
            ViewBag.BindProductCode = _commonRepository.GetProductCode().ToList();

            ResponseModel _response = new ResponseModel();
            if (ModelState.IsValid)
            {
                //if (ImageFile == null)
                //{
                //    ViewBag.Error = "Product Image is required.";
                //    return View();
                //}
                Int64 productSubCategoryId = productModel.ProductSubCategoryId == null ? 0 : productModel.ProductSubCategoryId.Value;
                Product _product = _productRepository.FindBy(x => x.ProductName.Trim().ToLower() == productModel.ProductName.Trim().ToLower() && x.ProductCategoryId == productModel.ProductCategoryId && x.ProductSubCategoryId == productSubCategoryId && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
                if (_product != null)
                {
                    ViewBag.Error = "Error: Product already exist";
                    return View();
                }

                try
                {
                    // Image byte converter
                    if (ImageFile != null)
                    {
                        productModel.DocProduct = new DocumentModel();
                        using (var memoryStream = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            productModel.DocProduct.Filebytes = imageBytes;
                            productModel.DocProduct.FileName = ImageFile.FileName;
                            productModel.DocProduct.FileExtenstion = Path.GetExtension(ImageFile.FileName);
                        }
                    }
                    using (var client = new HttpClient())
                    {
                        // Get UserId
                        productModel.CreatedBy = UserAuthenticate.UserId;
                        string apiURL = BaseUrl + "api/ApiProduct/EditProduct";
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.PostAsJsonAsync(apiURL, productModel).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            _response = (new JavaScriptSerializer()).Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                            // Return error message 
                            if (_response.Type.ToLower() == "error")
                            {
                                ViewBag.Error = _response.Message;
                                return View(productModel);
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
        /// Delete Product
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProduct(Int64 Id)
        {
            ResponseModel _response = new ResponseModel();
            var details = _productRepository.FindBy(x => x.ProductId == Id).FirstOrDefault();
            details.IsDeleted = true;
            details.ModifiedDate = DateTime.Now;
            details.ModifiedBy = UserAuthenticate.UserId;
            bool IsTrue = _productRepository.EditProduct(details);
            if (IsTrue)
            {
                _response.Type = "success";
                _response.Message = "Product deleted successfully";
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Somehing went wrong";
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region [Bind Dropdowns]

        [HttpPost]
        public ActionResult BindProductSubCategory(int Id)
        {
            var list = _productSubCategoryRepository.FindBy(x => x.ProductCategoryId == Id && x.Status == (int)Status.Active && x.IsDeleted == false).Select(x => new { x.ProductSubCategoryId, x.ProductSubCategoryName }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //---------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && _productCategoryRepository != null)
                _productCategoryRepository.Dispose();

            if (disposing && _productSubCategoryRepository != null)
                _productSubCategoryRepository.Dispose();

            if (disposing && _productRepository != null)
                _productRepository.Dispose();

            base.Dispose(disposing);
        }

    }
}
