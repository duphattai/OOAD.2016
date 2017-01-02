using LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarManager.Areas.Admin.Models
{
    public class BusStationModel
    {
        public int IdBusStation { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "BusStation", ResourceType = typeof(LocalResources.Resource))]
        public string Name { get; set; }
    }


    public class ChannelItemModel
    {
        public int IdChannel { get; set; }
        public string Channel { get; set; }
        public int RunTime { get; set; }

        public double DefaultPrice { get; set; }
    }

    public class ChannelFilterModel
    {
        public int? BusStationFrom { get; set; }
        public int? BusStationTo { get; set; }
    }

    public class ChannelModel
    {
        public int IdChannel { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "BusStationFrom", ResourceType = typeof(LocalResources.Resource))]
        public int IdBusStationFrom { get; set; }
        
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "BusStationTo", ResourceType = typeof(LocalResources.Resource))]
        public int IdBusStationTo { get; set; }

        [Range(0, int.MaxValue)]
        public int RunTime { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Price", ResourceType = typeof(LocalResources.Resource))]
        [Range(0, int.MaxValue)]
        public double DefaultPrice { get; set; }
    }
}