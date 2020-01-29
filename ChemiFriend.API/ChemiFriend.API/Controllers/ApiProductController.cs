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
using ChemiFriend.Utility;
using System.Collections.Generic;
using ChemiFriend.API.Models.ImageUploadModel;
using System.IO;
using ChemiFriend.API.Infrastructure;
using System.Web;
using ChemiFriend.Entity.JsonModel;

namespace ChemiFriend.API.Controllers
{
    //[APIAuthorizeAttribute]
    public class ApiProductController : ApiController
    {
        IProductCategoryRepository _productCategoryRepository;
        IProductSubCategoryRepository _productSubCategoryRepository;
        IProductRepository _productRepository;

        #region [Construtor]
        public ApiProductController()
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _productSubCategoryRepository = new ProductSubCategoryRepository();
            _productRepository = new ProductRepository();
        }
        #endregion

        #region [Product Category API's]

        /// <summary>
        /// Get Product Category List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetProductCategoryList()
        {
            ResponseWithData<List<ProductCategory>> _response = new ResponseWithData<List<ProductCategory>>();
            var _getList = _productCategoryRepository.FindBy(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
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
        /// Get Product Category By Id
        /// </summary>
        /// <param name="ProductCategoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetProductCategoryById(Int64 productCategoryId)
        {
            ResponseWithData<ProductCategory> _response = new ResponseWithData<ProductCategory>();
            var _getDetails = _productCategoryRepository.FindBy(x => x.ProductCategoryId == productCategoryId && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getDetails != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _getDetails;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Invalid Company";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Create Product Category
        /// </summary>
        /// <param name="productCategoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateProductCategory(ProductCategoryModel productCategoryModel)
        {
            ResponseModel _response = new ResponseModel();
            var _getCategory = _productCategoryRepository.FindBy(x => x.ProductCategoryName.Trim().ToLower() == productCategoryModel.ProductCategoryName.Trim().ToLower() && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getCategory != null)
            {
                _response.Type = "error";
                _response.Message = "Company already Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            var model = Mapper.Map<ProductCategoryModel, ProductCategory>(productCategoryModel);
            model.CreatedDate = DateTime.Now;
            model.Status = 1;
            model.IsDeleted = false;
            var Result = _productCategoryRepository.CreateProductCategory(model);
            if (Result != null)
            {
                // For Uploading image ----
                if (productCategoryModel.DocProductCategory != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(productCategoryModel.DocProductCategory);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        ProductCategory _productCategory = _productCategoryRepository.FindBy(x => x.ProductCategoryId == Result.ProductCategoryId).FirstOrDefault();
                        if (_productCategory != null)
                        {
                            _productCategory.ProductCategoryImagePath = uploadedFileName[0].ToString();
                            _productCategoryRepository.Update(_productCategory);
                            _productCategoryRepository.SaveChanges();
                        }
                    }
                }

                _response.Type = "success";
                _response.Message = "Company created successfully";
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
        /// Edit Product Category
        /// </summary>
        /// <param name="productCategoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditProductCategory(ProductCategoryModel productCategoryModel)
        {
            ResponseModel _response = new ResponseModel();
            var _getCategory = _productCategoryRepository.FindBy(x => x.ProductCategoryId != productCategoryModel.ProductCategoryId && x.ProductCategoryName.Trim().ToLower() == productCategoryModel.ProductCategoryName.Trim().ToLower() && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getCategory != null)
            {
                _response.Type = "error";
                _response.Message = "Company already Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            var _productCategory = _productCategoryRepository.FindBy(x => x.ProductCategoryId == productCategoryModel.ProductCategoryId).FirstOrDefault();
            _productCategory.ProductCategoryId = productCategoryModel.ProductCategoryId;
            _productCategory.ProductCategoryName = productCategoryModel.ProductCategoryName;
            _productCategory.ModifiedBy = productCategoryModel.ModifiedBy;
            _productCategory.ModifiedDate = productCategoryModel.ModifiedDate;

            // var model = Mapper.Map<ProductCategoryModel, ProductCategory>(productCategoryModel);
            var IsResponse = _productCategoryRepository.EditProductCategory(_productCategory);
            if (IsResponse)
            {
                // For Uploading image ----
                if (productCategoryModel.DocProductCategory != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(productCategoryModel.DocProductCategory);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        if (_productCategory != null)
                        {
                            // delete exiting images
                            string ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + _productCategory.ProductCategoryImagePath);
                            if (System.IO.File.Exists(ImgPath))
                            {
                                System.IO.File.Delete(ImgPath);
                            }
                            string ImgOriginalPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "Original_" + _productCategory.ProductCategoryImagePath);
                            if (System.IO.File.Exists(ImgOriginalPath))
                            {
                                System.IO.File.Delete(ImgOriginalPath);
                            }
                            string TImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "T" + _productCategory.ProductCategoryImagePath);
                            if (System.IO.File.Exists(TImgPath))
                            {
                                System.IO.File.Delete(TImgPath);
                            }
                            //------------------------------
                            _productCategory.ProductCategoryImagePath = uploadedFileName[0].ToString();
                            _productCategoryRepository.Update(_productCategory);
                            _productCategoryRepository.SaveChanges();
                        }
                    }
                }
                _response.Type = "success";
                _response.Message = "Company updated successfully";
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
        /// Delete Product Category
        /// </summary>
        /// <param name="productCategoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteProductCategory(Int64 productCategoryId, Int64 UserId)
        {
            ResponseModel _response = new ResponseModel();
            var _getCategory = _productCategoryRepository.FindBy(x => x.ProductCategoryId != productCategoryId).FirstOrDefault();
            if (_getCategory == null)
            {
                _response.Type = "error";
                _response.Message = "Company not Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            _getCategory.IsDeleted = true;
            _getCategory.ModifiedBy = UserId;

            var IsResponse = _productCategoryRepository.EditProductCategory(_getCategory);
            if (IsResponse)
            {
                _response.Type = "success";
                _response.Message = "Company deleted successfully";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Something went wrong";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        #endregion

        #region [Product SubCategory Methods]

        /// <summary>
        /// Get Product SubCategory List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetProductSubCategoryList()
        {
            ResponseWithData<List<GetProductSubCategoryModel>> _response = new ResponseWithData<List<GetProductSubCategoryModel>>();
            var _getList = _productSubCategoryRepository.GetProductSubCategoryList().Where(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
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
        /// Get Product SubCategory By Id
        /// </summary>
        /// <param name="ProductSubCategoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetProductSubCategoryById(Int64 productSubCategoryId)
        {
            ResponseWithData<ProductSubCategory> _response = new ResponseWithData<ProductSubCategory>();
            var _getDetails = _productSubCategoryRepository.FindBy(x => x.ProductSubCategoryId == productSubCategoryId && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getDetails != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _getDetails;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Invalid Brand";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Create Product SubCategory
        /// </summary>
        /// <param name="productSubCategoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateProductSubCategory(ProductSubCategoryModel productSubCategoryModel)
        {
            ResponseModel _response = new ResponseModel();
            var _getSubCategory = _productSubCategoryRepository.FindBy(x => x.ProductSubCategoryName.Trim().ToLower() == productSubCategoryModel.ProductSubCategoryName.Trim().ToLower() && x.ProductCategoryId == productSubCategoryModel.ProductCategoryId && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getSubCategory != null)
            {
                _response.Type = "error";
                _response.Message = "Brand already Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            var model = Mapper.Map<ProductSubCategoryModel, ProductSubCategory>(productSubCategoryModel);
            model.CreatedDate = DateTime.Now;
            model.Status = 1;
            model.IsDeleted = false;
            var Result = _productSubCategoryRepository.CreateProductSubCategory(model);
            if (Result != null)
            {
                // For Uploading image ----
                if (productSubCategoryModel.DocProductSubCategory != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(productSubCategoryModel.DocProductSubCategory);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        ProductSubCategory _productSubCategory = _productSubCategoryRepository.FindBy(x => x.ProductSubCategoryId == Result.ProductSubCategoryId).FirstOrDefault();
                        if (_productSubCategory != null)
                        {
                            _productSubCategory.ProductSubCategoryImagePath = uploadedFileName[0].ToString();
                            _productSubCategoryRepository.Update(_productSubCategory);
                            _productSubCategoryRepository.SaveChanges();
                        }
                    }
                }

                _response.Type = "success";
                _response.Message = "Brand created successfully";
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
        /// Edit Product SubCategory
        /// </summary>
        /// <param name="productSubCategoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditProductSubCategory(ProductSubCategoryModel productSubCategoryModel)
        {
            ResponseModel _response = new ResponseModel();
            var _getSubCategory = _productSubCategoryRepository.FindBy(x => x.ProductSubCategoryId != productSubCategoryModel.ProductSubCategoryId 
                                  && x.ProductCategoryId == productSubCategoryModel.ProductCategoryId && x.ProductSubCategoryName.Trim().ToLower() == productSubCategoryModel.ProductSubCategoryName.Trim().ToLower() 
                                  && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getSubCategory != null)
            {
                _response.Type = "error";
                _response.Message = "Brand already Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            // var model = Mapper.Map<ProductSubCategoryModel, ProductSubCategory>(productSubCategoryModel);
            var _productSubCategory = _productSubCategoryRepository.FindBy(x => x.ProductSubCategoryId == productSubCategoryModel.ProductSubCategoryId).FirstOrDefault();
            _productSubCategory.ProductSubCategoryId = productSubCategoryModel.ProductSubCategoryId;
            _productSubCategory.ProductCategoryId = productSubCategoryModel.ProductCategoryId;
            _productSubCategory.ProductSubCategoryName = productSubCategoryModel.ProductSubCategoryName;
            _productSubCategory.ModifiedBy = productSubCategoryModel.ModifiedBy;
            _productSubCategory.ModifiedDate = DateTime.Now;

            var IsResponse = _productSubCategoryRepository.EditProductSubCategory(_productSubCategory);
            if (IsResponse)
            {
                // For Uploading image ----
                if (productSubCategoryModel.DocProductSubCategory != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(productSubCategoryModel.DocProductSubCategory);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        if (_productSubCategory != null)
                        {
                            // delete exiting images
                            string ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + _productSubCategory.ProductSubCategoryImagePath);
                            if (System.IO.File.Exists(ImgPath))
                            {
                                System.IO.File.Delete(ImgPath);
                            }
                            string ImgOriginalPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "Original_" + _productSubCategory.ProductSubCategoryImagePath);
                            if (System.IO.File.Exists(ImgOriginalPath))
                            {
                                System.IO.File.Delete(ImgOriginalPath);
                            }
                            string TImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "T" + _productSubCategory.ProductSubCategoryImagePath);
                            if (System.IO.File.Exists(TImgPath))
                            {
                                System.IO.File.Delete(TImgPath);
                            }
                            //------------------------------
                            _productSubCategory.ProductSubCategoryImagePath = uploadedFileName[0].ToString();
                            _productSubCategoryRepository.Update(_productSubCategory);
                            _productSubCategoryRepository.SaveChanges();
                        }
                    }
                }

                _response.Type = "success";
                _response.Message = "Brand updated successfully";
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
        /// Delete Product SubCategory
        /// </summary>
        /// <param name="productSubCategoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteProductSubCategory(Int64 productSubCategoryId, Int64 UserId)
        {
            ResponseModel _response = new ResponseModel();
            var _getSubCategory = _productSubCategoryRepository.FindBy(x => x.ProductSubCategoryId != productSubCategoryId).FirstOrDefault();
            if (_getSubCategory == null)
            {
                _response.Type = "error";
                _response.Message = "Brand not Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            _getSubCategory.IsDeleted = true;
            _getSubCategory.ModifiedBy = UserId;

            var IsResponse = _productSubCategoryRepository.EditProductSubCategory(_getSubCategory);
            if (IsResponse)
            {
                _response.Type = "success";
                _response.Message = "Brand deleted successfully";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Something went wrong";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        #endregion

        #region [Product Methods]

        /// <summary>
        /// Get Product List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetProductList()
        {
            ResponseWithData<List<GetProductModel>> _response = new ResponseWithData<List<GetProductModel>>();
            var _getList = _productRepository.GetProductList().Where(x => x.Status == (int)Status.Active && x.IsDeleted == false).ToList();
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
        /// Get Product By Id
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetProductById(Int64 productId)
        {
            ResponseWithData<Product> _response = new ResponseWithData<Product>();
            var _getDetails = _productRepository.FindBy(x => x.ProductId == productId && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getDetails != null)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = _getDetails;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Invalid product";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateProduct(ProductModel productModel)
        {
            ResponseModel _response = new ResponseModel();
            var _getProduct = _productRepository.FindBy(x => x.ProductName.Trim().ToLower() == productModel.ProductName.Trim().ToLower() && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getProduct != null)
            {
                _response.Type = "error";
                _response.Message = "Product already Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            var model = Mapper.Map<ProductModel, Product>(productModel);
            model.CreatedDate = DateTime.Now;
            model.Status = 1;
            model.IsDeleted = false;
            var Result = _productRepository.CreateProduct(model);
            if (Result != null)
            {
                // For Uploading image ----
                if (productModel.DocProduct != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(productModel.DocProduct);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        Product _product = _productRepository.FindBy(x => x.ProductId == Result.ProductId).FirstOrDefault();
                        if (_product != null)
                        {
                            _product.ProductImagePath = uploadedFileName[0].ToString();
                            _productRepository.Update(_product);
                            _productRepository.SaveChanges();
                        }
                    }
                }
                _response.Type = "success";
                _response.Message = "Product created successfully";
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
        /// Edit Product
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditProduct(ProductModel productModel)
        {
            ResponseModel _response = new ResponseModel();
            var _getProduct = _productRepository.FindBy(x => x.ProductId != productModel.ProductId && x.ProductSubCategoryId == productModel.ProductSubCategoryId 
                                && x.ProductCategoryId == productModel.ProductCategoryId && x.ProductName.Trim().ToLower() == productModel.ProductName.Trim().ToLower() 
                                && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
            if (_getProduct != null)
            {
                _response.Type = "error";
                _response.Message = "Product already Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            //var model = Mapper.Map<ProductModel, Product>(productModel);
            var _product = _productRepository.FindBy(x => x.ProductId == productModel.ProductId).FirstOrDefault();
            _product.ProductId = productModel.ProductId;
            _product.ProductCategoryId = productModel.ProductCategoryId;
            _product.ProductSubCategoryId = productModel.ProductSubCategoryId;
            _product.ProductName = productModel.ProductName;
            _product.ProductTypeId = productModel.ProductTypeId;
            _product.MarketingCompany = productModel.MarketingCompany;
            _product.MRP = productModel.MRP;
            _product.PTR = productModel.PTR;
            _product.ProductCodeId = productModel.ProductCodeId;
            _product.ModifiedBy = productModel.ModifiedBy == null ? productModel.CreatedBy : productModel.ModifiedBy;
            _product.ModifiedDate = DateTime.Now;

            var IsResponse = _productRepository.EditProduct(_product);
            if (IsResponse)
            {
                // For Uploading image ----
                if (productModel.DocProduct != null)
                {
                    UploadDocumentModel UploadDocument = new UploadDocumentModel();
                    UploadDocument.documents = new List<DocumentModel>();
                    UploadDocument.ImageSize = 250;
                    UploadDocument.ImageQuality = 250;
                    UploadDocument.documents.Add(productModel.DocProduct);
                    List<string> uploadedFileName = SavePicture(UploadDocument);
                    if (uploadedFileName.Count > 0)
                    {
                        if (_product != null)
                        {
                            // delete exiting images
                            string ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + _product.ProductImagePath);
                            if (System.IO.File.Exists(ImgPath))
                            {
                                System.IO.File.Delete(ImgPath);
                            }
                            string ImgOriginalPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "Original_" + _product.ProductImagePath);
                            if (System.IO.File.Exists(ImgOriginalPath))
                            {
                                System.IO.File.Delete(ImgOriginalPath);
                            }
                            string TImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/UploadImg/" + "T" + _product.ProductImagePath);
                            if (System.IO.File.Exists(TImgPath))
                            {
                                System.IO.File.Delete(TImgPath);
                            }
                            //------------------------------
                            _product.ProductImagePath = uploadedFileName[0].ToString();
                            _productRepository.Update(_product);
                            _productRepository.SaveChanges();
                        }
                    }
                }

                _response.Type = "success";
                _response.Message = "Product updated successfully";
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
        /// Delete Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteProduct(Int64 productId, Int64 UserId)
        {
            ResponseModel _response = new ResponseModel();
            var _getProduct = _productRepository.FindBy(x => x.ProductId != productId).FirstOrDefault();
            if (_getProduct == null)
            {
                _response.Type = "error";
                _response.Message = "Product not Exist";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }

            _getProduct.IsDeleted = true;
            _getProduct.ModifiedBy = UserId;
            var IsResponse = _productRepository.EditProduct(_getProduct);
            if (IsResponse)
            {
                _response.Type = "success";
                _response.Message = "Product deleted successfully";
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Something went wrong";
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