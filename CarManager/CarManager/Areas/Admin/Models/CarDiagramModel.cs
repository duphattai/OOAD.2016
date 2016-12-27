using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LocalResources;

namespace CarManager.Areas.Admin.Models
{

    public class CarDiagramItemModel
    {
        public int IdCarDiagram { get; set; }
        public string Name { get; set; }
        public int NumberFloors { get; set; }
        public string SeatDiagram { get; set; }
        public string TypeSeat { get; set; }
    }

    public class CarDiagramModel
    {
        public int IdCarDiagram { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "CarDiagram", ResourceType = typeof(LocalResources.Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "NumberFloors", ResourceType = typeof(LocalResources.Resource))]
        public int NumberFloors { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "SeatDiagram", ResourceType = typeof(LocalResources.Resource))]
        public string SeatDiagram { get; set; }

        
        [Display(Name = "TypeSeat", ResourceType = typeof(LocalResources.Resource))]
        public string TypeSeat { get; set; }

        public CarDiagramModel()
        {
            NumberFloors = 1;
        }
    }
}