using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMaxiFarmacia.classHelper;

namespace WebMaxiFarmacia
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.maxifarmaciabdContext, Migrations.Configuration>());
            checkRolesSuperUser();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void checkRolesSuperUser()
        {
            UserHelper.CheckRole("SuperAdmin");
            UserHelper.CheckRole("Admin");
            UserHelper.CheckRole("User");


            UserHelper.CheckSuperUser();
        }
    }
}
