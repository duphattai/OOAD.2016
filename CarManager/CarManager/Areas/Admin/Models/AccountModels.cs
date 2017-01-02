using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LocalResources;
using System.Web.Mvc;

namespace CarManager.Areas.Admin.Models
{

    public class LoginViewModel
    {
        [Display(Name = "Username", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Pass { get; set; }
    }

    public class AccountItemModel
    {
        public int IdAccount { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string IdRoles { get; set; }
        public string FullName { get; set; }
        public string CMND { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class FilterAccountModel
    {
        public int? IdRole { get; set; }
    }

    public class AccountModel
    {        
        public int IdAccount { get; set; }
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Username", ResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthErrorMessage", ErrorMessageResourceType = typeof(Resource), MinimumLength = 5)]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessageResourceName = "InputSpecialCharacter", ErrorMessageResourceType = typeof(Resource))]
        [Remote("CheckUserNameExist", "Account", ErrorMessageResourceName = "UsernameExistedError", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Pass { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [System.ComponentModel.DataAnnotations.Compare("Pass", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        public string ConfirmPass { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Role", ResourceType = typeof(Resource))]
        public int[] SelectedValues { get; set; }
       
        public string IdRoles { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "FullName", ResourceType = typeof(Resource))]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceName = "InputSpecialCharacter", ErrorMessageResourceType = typeof(Resource))]
        public string CMND { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class AccountEditModel
    {
        public int IdAccount { get; set; }
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Username", ResourceType = typeof(Resource))]        
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Pass { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Role", ResourceType = typeof(Resource))]
        public int[] SelectedValues { get; set; }

        public string IdRoles { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "FullName", ResourceType = typeof(Resource))]
        public string FullName { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceName = "InputSpecialCharacter", ErrorMessageResourceType = typeof(Resource))]
        public string CMND { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}