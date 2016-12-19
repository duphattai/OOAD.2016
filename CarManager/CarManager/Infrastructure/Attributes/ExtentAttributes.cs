using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarManager.Infrastructure.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        
        private readonly string[] allowedRoles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserRoles"] == null)
            {
                return false;
            }

            bool authorize = false;
            string[] userRoles = (string[])httpContext.Session["UserRoles"];

            authorize = userRoles.Where(t => allowedRoles.Contains(t)).Count() != 0;
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // User doesn't login
            if (filterContext.HttpContext.Session["UserRoles"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary { { "controller", "Login" }, { "action", "AccessDenied" } });
            }
        }
    }  
}