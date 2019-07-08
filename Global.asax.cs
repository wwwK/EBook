using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Web.SessionState;
using EBook.Models;

namespace EBook
{
    
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        { 
            AreaRegistration.RegisterAllAreas();
            
            //调用web API
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
//            Database.SetInitializer(new DropCreateDatabaseAlways<OracleDbContext>());
            //表项更改后启用该句重新建表

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            
        }
        

    }
}