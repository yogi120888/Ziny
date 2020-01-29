using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemiFriend.Web.Models.Response
{
    public class ResponseWithData<T> : ResponseModel
    {
        public T data { get; set; }
    }
}