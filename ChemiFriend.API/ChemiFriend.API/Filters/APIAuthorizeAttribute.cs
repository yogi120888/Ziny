using ChemiFriend.Data.Repository;
using ChemiFriend.Utility;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ChemiFriend.API.Filters
{
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {
        IUserMasterRepository _user = new UserMasterRepository();
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }

            base.OnAuthorization(filterContext);
            // HandleUnauthorizedRequest(filterContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            // base.HandleUnauthorizedRequest(filterContext);
            base.HandleUnauthorizedRequest(filterContext);
            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Unauthorized;
            response.Content = new StringContent(Json.Encode(new { Status = "False", Message = "Unauthorized" }), Encoding.UTF8, "application/json");
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            bool validFlag = false;
            string error = "";
            try
            {
                string token = actionContext.Request.Headers.GetValues("Token").First();
                //if (!string.IsNullOrEmpty(token))
                //{
                //    string key = CommonHelper.Decrypt(token);
                //    string[] parts = key.Split(new char[] { '|' });
                //    if (parts.Length > 1)
                //    {
                //        int UserId = Convert.ToInt32(parts[0]);
                //        DateTime expireon = Convert.ToDateTime(parts[1]);
                //        var TokenDetails = _user.GetTokenDetails(UserId);
                //        if (TokenDetails != null)
                //        {
                //            string Token = CommonHelper.Decrypt(TokenDetails.Token);
                //            if (key.Equals(Token))
                //            {
                //                validFlag = true;
                //            }
                //            else
                //            {
                //                validFlag = false;
                //            }
                //        }
                //        //if ((DateTime.Now > expireon))
                //        //{
                //        //    validFlag = false;
                //        //}
                //        //else
                //        //{
                //        //    validFlag = true;
                //        //}
                //    }
                //}
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return validFlag;
        }

    }
}