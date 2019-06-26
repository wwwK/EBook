using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
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
            
<<<<<<< Updated upstream
            //Database.SetInitializer(new DropCreateDatabaseAlways<OracleDbContext>());
            
=======
         //   Database.SetInitializer(new DropCreateDatabaseAlways<DbContext>());
         
>>>>>>> Stashed changes
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
        
        protected void Session_Start(object sender, EventArgs e)
        {
           
        }
 
        protected void Session_End(object sender, EventArgs e)
        {
            
        } 

    }
}