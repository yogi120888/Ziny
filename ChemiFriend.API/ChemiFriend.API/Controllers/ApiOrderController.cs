using AutoMapper;
using ChemiFriend.API.Models.InputModel;
using ChemiFriend.API.Models.Response;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Data.Repository;
using ChemiFriend.Entity;
using ChemiFriend.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChemiFriend.API.Controllers
{
    public class ApiOrderController : ApiController
    {
        IOrderRepository _orderRepository;
        IOrderItemRepository _orderItemRepository;

        #region [Construtor]
        public ApiOrderController()
        {
            _orderRepository = new OrderRepository();
            _orderItemRepository = new OrderItemRepository();
        }
        #endregion

        #region [Public Methods for Order]

        /// <summary>
        /// Get Order Details
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetOrderDetails(Int64 OrderId)
        {
            ResponseWithData<Order> _response = new ResponseWithData<Order>();
            var _getDetails = _orderRepository.FindBy(x => x.OrderId == OrderId && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
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
                _response.Message = "Invalid Order";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateOrder(OrderModel orderModel)
        {
            ResponseWithData<Order> _response = new ResponseWithData<Order>();

            var model = Mapper.Map<OrderModel, Order>(orderModel);
            model.CreatedDate = DateTime.Now;
            model.Status = 1;
            model.IsDeleted = false;
            var Result = _orderRepository.SaveReturnModel(model);
            if (Result != null)
            {
                _response.Type = "success";
                _response.Message = "Order created successfully";
                _response.data = Result;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Something went wrong";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        #endregion

        #region [Public Methods for Order Item]

        /// <summary>
        /// Get Order Item Details
        /// </summary>
        /// <param name="OrderItemId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetOrderItemDetails(Int64 OrderItemId)
        {
            ResponseWithData<OrderItem> _response = new ResponseWithData<OrderItem>();
            var _getDetails = _orderItemRepository.FindBy(x => x.OrderItemId == OrderItemId && x.Status == (int)Status.Active && x.IsDeleted == false).FirstOrDefault();
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
                _response.Message = "Invalid Order Item";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        [HttpPost]
        public HttpResponseMessage CreateOrderItem(OrderItemModel orderItemModel)
        {
            ResponseWithData<OrderItem> _response = new ResponseWithData<OrderItem>();

            var model = Mapper.Map<OrderItemModel, OrderItem>(orderItemModel);
            model.CreatedDate = DateTime.Now;
            model.Status = 1;
            model.IsDeleted = false;
            var Result = _orderItemRepository.SaveReturnModel(model);
            if (Result != null)
            {
                _response.Type = "success";
                _response.Message = "Order Item created successfully";
                _response.data = Result;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
            else
            {
                _response.Type = "error";
                _response.Message = "Something went wrong";
                _response.data = null;
                return Request.CreateResponse(HttpStatusCode.OK, _response);
            }
        }

        #endregion



    }

}
