using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LocalResources;
using System.Web.Mvc;

namespace CarManager.Areas.Admin.Models
{

    public class ReportModel
    {
        [Display(Name = "Username", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int TOTAL_TICKED { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public double TOTAL_PRICE { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalTicked { get; set; }
        public double TotalPrice { get; set; }
    }
  
}