using CarManager.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.Service;


namespace CarManager.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;


        public AccountController(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }


        #region Login, Logout, Access denied

        // GET: Admin/Login
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
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

                    if (string.IsNullOrEmpty(ReturnUrl))
                        return RedirectToAction("Index", "DashBoard");
                    else
                        return Redirect(ReturnUrl);
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
            return RedirectToAction("Login");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}