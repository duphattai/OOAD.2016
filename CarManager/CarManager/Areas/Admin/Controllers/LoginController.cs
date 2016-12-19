using CarManager.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.Service;


namespace CarManager.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;


        public LoginController(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }


        // GET: Admin/Login
        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // get account by username, pass
                var account = _accountService.DoesAccountExist(model.UserName, model.Pass);
                if (account != null)
                {
                    Session["Username"] = account.UserName;
                    Session["ID"] = account.IdAccount;
                    
                    // get user roles
                    int[] userRoles = account.IdRoles.Split(',').Select(o=> int.Parse(o)).ToArray();
                    Session["UserRoles"] = _roleService.GetAll().Where(t => userRoles.Contains(t.IdRole)).Select(o => o.RoleName).ToArray();
                    
                    return RedirectToAction("Index", "DashBoard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, LocalResources.Resource.LoginFailedMessage);
                    return View(model);
                }
            }
            else
                return View(model);
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}