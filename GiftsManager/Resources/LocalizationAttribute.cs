using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace GiftsManager.Resources
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string lang = (string)filterContext.RouteData.Values["lang"] ?? "en";

            try
            {
                Thread.CurrentThread.CurrentCulture =
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            catch (Exception e)
            {
                throw new NotSupportedException(string.Format("ERROR: Invalid language code '{0}'.", lang));
            }
        }
    }
}