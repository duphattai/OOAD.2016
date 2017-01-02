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


    public class ChannelModel
    {
    }
}