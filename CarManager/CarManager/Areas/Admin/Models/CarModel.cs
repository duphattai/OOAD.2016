using LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarManager.Areas.Admin.Models
{

    public class CarItemModel
    {
        public int IdCar { get; set; }
        public string CarNumberPlate { get; set; }
        public int TotalSeat { get; set; }
        public int IdCarDiagram { get; set; }
        public string CarDiagramName { get; set; }
        public string TypeSeat { get; set; }
    }

    public class FilterCarModel
    {
        public int? IdCarDiagram { get; set; }
    }

    public class CarModel
    {
        public int IdCar { get; set; }
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "CarNumberPlate", ResourceType = typeof(LocalResources.Resource))]
        public string CarNumberPlate { get; set; }

        [Display(Name = "TotalSeat", ResourceType = typeof(LocalResources.Resource))]
        public int TotalSeat { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "CarDiagram", ResourceType = typeof(LocalResources.Resource))]
        public int IdCarDiagram { get; set; }   
    }

    public class SeatChartModel
    {
        public int NumberFloors { get; set; }
        public List<IEnumerable<string>> SeatsList { get; set; }

        public SeatChartModel()
        {
            NumberFloors = 0;
            SeatsList = new List<IEnumerable<string>>();
        }
    }
}