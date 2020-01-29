using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace ChemiFriend.Web.Helpers
{
    public class CustomDateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            CultureInfo culture = new CultureInfo("en-GB"); // dd/MM/yyyy
            var date = value.ConvertTo(typeof(DateTime), culture);
            return date;
        }
    }
}