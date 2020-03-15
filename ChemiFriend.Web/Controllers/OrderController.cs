using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.Models;
using ChemiFriend.Utility;
using ChemiFriend.Web.Filters;
using ChemiFriend.Web.Models.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemiFriend.Web.Controllers
{
    [CustomAuthorize(Roles = "Admin,Distributor,Retrailer")]
    public class OrderController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        string BaseUrl = ConfigurationManager.AppSettings["APIBaseUrl"].ToString();

        IUserMasterRepository _user;
        IcommonRepository _commonRepository;
        IDealRepository _dealRepository;
        ISchemeRepository _schemeRepository;
        IOrderRepository _orderRepository;
        IOrderItemRepository _orderItemRepository;

        public OrderController()
        {
            _user = new UserMasterRepository();
            _commonRepository = new commonRepository();
            _dealRepository = new DealRepository();
            _schemeRepository = new SchemeRepository();
            _orderRepository = new OrderRepository();
            _orderItemRepository = new OrderItemRepository();
        }

        /// <summary>
        /// Customer Order List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CustomerOrderList()
        {
            List<GetCustomerOrderModel> getCustomerOrderModels = new List<GetCustomerOrderModel>();
            if (UserAuthenticate.Role == (int)Roles.Admin)
            {
                getCustomerOrderModels = _orderRepository.GetOrderList().Where(x => x.IsDeleted == false).ToList();
            }
            else if (UserAuthenticate.Role == (int)Roles.Retrailer)
            {
                getCustomerOrderModels = _orderRepository.GetOrderList().Where(x => x.UserId == UserAuthenticate.UserId && x.IsDeleted == false).ToList();
            }

            return View(getCustomerOrderModels);
        }

        /// <summary>
        /// Order Items List [Admin, Distributor and Reailser sell purchase reports]
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderItemsList(Int64 OrderId = 0, string AsCustomer = "")
        {
            List<GetOrderItemModel> getOrderItemModels = new List<GetOrderItemModel>();
            if (UserAuthenticate.Role == (int)Roles.Admin && OrderId > 0)
            {
                getOrderItemModels = _orderItemRepository.GetOrderItemList().Where(x => x.OrderId == OrderId && x.IsDeleted == false).ToList();
            }
            else if (UserAuthenticate.Role == (int)Roles.Distributor)
            {
                // Sell Products by Distributor
                if(AsCustomer == "yes")
                {
                    getOrderItemModels = _orderItemRepository.GetOrderItemList().Where(x => x.UserId == UserAuthenticate.UserId && x.IsDeleted == false).ToList();
                }
                else
                {
                    getOrderItemModels = _orderItemRepository.GetOrderItemList().Where(x => x.CreatedBy == UserAuthenticate.UserId && x.IsDeleted == false).ToList();
                }
            }
            else if (UserAuthenticate.Role == (int)Roles.Retrailer)
            {
                // purchase Products by Retrailer
                getOrderItemModels = _orderItemRepository.GetOrderItemList().Where(x => x.UserId == UserAuthenticate.UserId && x.IsDeleted == false).ToList();
            }

            return View(getOrderItemModels);
        }

        /// <summary>
        /// Update Order Item Status
        /// </summary>
        /// <param name="OrderItemId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateOrderItemStatus(Int64 OrderItemId, byte Status)
        {
            ResponseModel _response = new ResponseModel();
            var details = _orderItemRepository.FindBy(x => x.OrderItemId == OrderItemId).FirstOrDefault();
            details.Status = Status;
            details.ModifiedDate = DateTime.Now;
            details.ModifiedBy = UserAuthenticate.UserId;
            _orderItemRepository.Update(details);
            int rows = _orderItemRepository.SaveChanges();
            if (rows > 0)
            {
                _response.Type = "success";
                _response.Message = "Updated successfully";
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Somehing went wrong";
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _orderRepository != null)
                _orderRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}
