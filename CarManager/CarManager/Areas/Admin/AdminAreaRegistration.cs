﻿using System.Web.Mvc;

namespace CarManager.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "Admin_Login",
               "Admin/Login",
               new { action = "Login", controller = "Account" }
            );


            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", controller = "DashBoard", id = UrlParameter.Optional }
            );
        }
    }
}