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
            string controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            string action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();

            // doesn't login
            if (httpContext.Session["UserRoles"] == null)
            {
                // calling login page, return true
                if (controller == "Account" && action == "Login")
                    return true;

                return false;
            }
            else // login
            {
                string[] userRoles = (string[])httpContext.Session["UserRoles"];

                bool authorize = userRoles.Where(t => allowedRoles.Contains(t)).Count() != 0;
                return authorize;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // User doesn't login
            if (filterContext.HttpContext.Session["UserRoles"] == null)
            {
                
                string returnUrl = filterContext.RequestContext.HttpContext.Request.CurrentExecutionFilePath;
                filterContext.Result = new RedirectToRouteResult("Admin_Login", new RouteValueDictionary ( new {returnUrl = returnUrl} ));
            }
            else // Login and access denied
            {
                filterContext.Result = new ViewResult { ViewName = "~/Areas/Admin/Views/Shared/AccessDenied.cshtml" };
            }
        }
    }  
}