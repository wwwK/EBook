﻿using System;
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
            
           // Database.SetInitializer(new DropCreateDatabaseAlways<OracleDbContext>());
            

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }


        public override void Init()
        {
            this.PostAuthenticateRequest += (sender, e) =>
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
           
        }
 
        protected void Session_End(object sender, EventArgs e)
        {
            
        } 

    }
}