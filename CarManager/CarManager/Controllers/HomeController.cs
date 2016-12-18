using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.Service;

namespace CarManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoleService _roleService;
        public HomeController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public ActionResult Index()
        {
            ViewBag.AllRoles = string.Join("; ", _roleService.GetAll().Select(o=>o.RoleName));
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}