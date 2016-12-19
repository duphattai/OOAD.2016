using CarManager.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarManager.Areas.Admin.Controllers
{
    [CustomAuthorize("Administration", "Salesman", "Manager")]
    public class BaseController : Controller
    {
        protected int _pageSize;

        // GET: Admin/Base
        public BaseController()
        {
            _pageSize = 15;
        }


        //
        // Summary:
        //     Called before the action method is invoked.
        //
        // Parameters:
        //   filterContext:
        //     Information about the current request and action.
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // if user not loggin
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = filterContext.ActionDescriptor.ActionName;

            if (Session["UserRoles"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}