using System.Web;
using System.Web.Mvc;
using GiftsManager.Helper;

namespace GiftsManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
