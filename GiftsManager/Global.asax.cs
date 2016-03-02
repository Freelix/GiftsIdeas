using GiftsManager.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GiftsManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DeleteDatabase();
            InitialiseDatabase();
        }

        private void DeleteDatabase()
        {
            using (var ctx = new DataBaseContext())
            {
                ctx.Database.Delete();
            }
        }

        private void InitialiseDatabase()
        {
            IDatabaseInitializer<DataBaseContext> init = new InitGiftsManager();
            Database.SetInitializer(init);
            init.InitializeDatabase(new DataBaseContext());
        }
    }
}
