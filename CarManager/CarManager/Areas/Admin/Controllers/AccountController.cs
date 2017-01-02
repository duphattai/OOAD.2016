using CarManager.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.Service;
using AutoMapper;
using PagedList;
using DataLayer;

namespace CarManager.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IRoleService roleService, IMapper mapper)
        {
            _accountService = accountService;
            _roleService = roleService;
            _mapper = mapper;
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

        public ActionResult Index()
        {
            var role = _roleService.GetAll();
            ViewBag.Role = new SelectList(_roleService.GetAll(), "IdRole", "RoleName");

            return View(new FilterAccountModel());
        }

        [HttpGet]
        public PartialViewResult AccountList(FilterAccountModel filter, int page = 1)
        {
            var temp = _accountService.GetList(filter.IdRole);
            var model = _mapper.Map<IEnumerable<AccountItemModel>>(temp);
            model = model.ToPagedList(page, _pageSize);

            foreach (var item in model)
            {               
                int[] idRoles = item.IdRoles.Split(',').Select(o => int.Parse(o)).ToArray();
                item.IdRoles = GetRoleName(idRoles);               
            }
            ViewBag.Role = filter.IdRole;

            return PartialView(model);
        }

        string GetRoleName(int[] idRoles)
        {           
            string[] name = new string[idRoles.Count()];
            var i = 0;
            foreach (var item in idRoles)
            {
                var role = _roleService.Get(item);
                name[i] = role.RoleName;
                i++;
            }
            
            return string.Join(",",name);
        }

        
        public ActionResult Create()
        {
            var role = _roleService.GetAll();
            ViewBag.Role = new SelectList(_roleService.GetAll(), "IdRole", "RoleName");

            ViewBag.IsInsert = true;
            return View("Insert", new AccountModel());
        }

        [HttpPost]
        public ActionResult Create(AccountModel model)
        {
            ViewBag.Role = new SelectList(_roleService.GetAll(), "IdRole", "RoleName");
            ViewBag.IsInsert = true;
            model.IdRoles = string.Join(",", model.SelectedValues);
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Account>(model);
                string errorInsert = _accountService.Insert(entity);

                if (errorInsert != null)
                {
                    ModelState.AddModelError(string.Empty, errorInsert);
                    return View("Insert", model);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, LocalResources.Resource.CannotInsertData);
                return View("Insert", model);
            }
        }


        public ActionResult Edit(int id)
        {
            ViewBag.Role = new SelectList(_roleService.GetAll(), "IdRole", "RoleName");
            
            ViewBag.IsInsert = false;
            var model = _mapper.Map<AccountEditModel>(_accountService.Get(id));

            ViewBag.AccountRole = model.IdRoles.Split(',').Select(o => int.Parse(o)).ToArray();

            return View("Update", model);
        }

        [HttpPost]
        public ActionResult Edit(AccountEditModel model)
        {
            ViewBag.Role = new SelectList(_roleService.GetAll(), "IdRole", "RoleName");
            ViewBag.IsInsert = false;
            model.IdRoles = string.Join(",", model.SelectedValues);
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Account>(model);
                string error = _accountService.Update(entity);

                if (error != null)
                {
                    ModelState.AddModelError(string.Empty, error);
                    return View("Update", model);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, LocalResources.Resource.CannotInsertData);
                ViewBag.AccountRole = model.IdRoles.Split(',').Select(o => int.Parse(o)).ToArray();
                return View("Update", model);
            }
        }


        public ActionResult Detail(int id)
        {
            ViewBag.Role = new SelectList(_roleService.GetAll(), "IdRole", "RoleName");
            var model = _mapper.Map<AccountModel>(_accountService.Get(id));
            ViewBag.AccountRole = model.IdRoles.Split(',').Select(o => int.Parse(o)).ToArray();
            return View(model);
        }

        public ActionResult Delete(int id, int page = 1)
        {
            string error = _accountService.Delete(id);
            if (error != null)
            {
                return Content(null);
            }

            return RedirectToAction("AccountList", new { page = page });
        }


        public ActionResult CheckUserNameExist(string UserName)
        {
            bool ifUserNameExist = false;
            try
            {
                ifUserNameExist = _accountService.DoesUserNameExist(UserName) ? true : false;
                return Json(!ifUserNameExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        bool DoesUserNameExist(string username)
        {
            var check = _accountService.DoesUserNameExist(username);

            if (check == true)
                return true;
            else
                return false;
        }
    }
}