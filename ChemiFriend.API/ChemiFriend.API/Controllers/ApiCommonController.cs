using ChemiFriend.API.Models;
using ChemiFriend.API.Models.InputModel;
using ChemiFriend.API.Models.Response;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity;
using ChemiFriend.ENTITY;
using ChemiFriend.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChemiFriend.API.Controllers
{
    public class ApiCommonController : ApiController
    {
        IcommonRepository _common;
        public ApiCommonController()
        {
            _common = new commonRepository();
        }

        /// <summary>
        /// Common Binder
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage CommonBinder(string Ids)
        {
            CommonBinderModel model = new CommonBinderModel();
            string[] arr = Ids.Split(',');
            if (arr.Length > 0)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (Convert.ToInt32(arr[i]) == (int)CommonBinding.State)
                    {
                        model.lstState = _common.GetState(1).ToList();
                    }
                    if (Convert.ToInt32(arr[i]) == (int)CommonBinding.Roles)
                    {
                        model.lstRole = _common.GetUserRoleList().ToList();
                    }
                }
                if (model != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { Type = "success", Message = "Success", data = model });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { Type = "error", Message = "No Record Found", data = model });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { Type = "error", Message = "No Record Found", data = model });
            }
        }


        /// <summary>
        /// Get Country List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCountryList()
        {
            ResponseWithData<List<Country>> _response = new ResponseWithData<List<Country>>();
            var list = _common.GetCountry().ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get State List
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetStateList(int CountryId)
        {
            ResponseWithData<List<State>> _response = new ResponseWithData<List<State>>();
            var list = _common.GetState(CountryId).ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Get User Role List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserRoleList()
        {
            ResponseWithData<List<UserRole>> _response = new ResponseWithData<List<UserRole>>();
            var list = _common.GetUserRoleList().ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        [HttpGet]
        public HttpResponseMessage GeSchemeTypeList()
        {
            ResponseWithData<List<SchemeType>> _response = new ResponseWithData<List<SchemeType>>();
            var list = _common.GetSchemeType().ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        [HttpGet]
        public HttpResponseMessage GeDealTypeList()
        {
            ResponseWithData<List<DealType>> _response = new ResponseWithData<List<DealType>>();
            var list = _common.GetDealType().ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        [HttpGet]
        public HttpResponseMessage GeFormTypeList()
        {
            ResponseWithData<List<FormType>> _response = new ResponseWithData<List<FormType>>();
            var list = _common.GetFormType().ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        [HttpGet]
        public HttpResponseMessage GePackTypeList()
        {
            ResponseWithData<List<PackType>> _response = new ResponseWithData<List<PackType>>();
            var list = _common.GetPackType().ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        [HttpGet]
        public HttpResponseMessage GeProductTypeList()
        {
            ResponseWithData<List<ProductType>> _response = new ResponseWithData<List<ProductType>>();
            var list = _common.GetProductType().ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetGSTApplicableList()
        {
            ResponseWithData<List<GSTApplicable>> _response = new ResponseWithData<List<GSTApplicable>>();
            var list = _common.GetGSTApplicable().ToList();
            if (list.Count > 0)
            {
                _response.Type = "success";
                _response.Message = "success";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "No Record Found";
                _response.data = list;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }


    }
}
