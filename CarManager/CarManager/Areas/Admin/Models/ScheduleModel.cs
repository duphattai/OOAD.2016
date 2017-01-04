using DataLayer;
using LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarManager.Areas.Admin.Models
{
    public class ScheduleFilterModel
    {
        public int? IdChannel { get; set; }
        public string StartDate { get; set; }
    }

    public class ScheduleItemModel
    {
        public int IdSchedule { get; set; }
        public DateTime StartTime { get; set; }
        public double Price { get; set; }

        public virtual Car Car { get; set; }
        public virtual Channel Channel { get; set; }
    }

    public class ScheduleModel
    {
        public int? IdSchedule { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Channel", ResourceType = typeof(LocalResources.Resource))]
        public int IdChannel { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Car", ResourceType = typeof(LocalResources.Resource))]
        public int IdCar { get; set; }

        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{ HH:mm dd/MM/yyyy }")]
        [Display(Name = "StartTime", ResourceType = typeof(Resource))]
        public System.DateTime StartTime { get; set; }
        
        public System.DateTime ArrivalTime { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "CurrentPrice", ResourceType = typeof(LocalResources.Resource))]
        [Range(0, int.MaxValue)]
        public double Price { get; set; }

        public string Driver { get; set; }
        public int? TotalSeat { get; set; }

        public ScheduleModel()
        {
            IdSchedule = 0;
            StartTime = DateTime.Now;
        }
    }
}