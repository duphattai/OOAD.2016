using LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarManager.Areas.Admin.Models
{

    public class OrderItemModel
    {
        public int? IdSchedule { get; set; }
        public int IdOrder { get; set; }
        public string OrderName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string Seats { get; set; }
        public string Note { get; set; }
        public string PickUp { get; set; }
    }

    public class OrderFilterModel
    {
        public int? IdChannel { get; set; }
        public string SearchString { get; set; }
        public DateTime StartDate { get; set; }
        public string Phone { get; set; }
    }

    public class OrderModel
    {
        public int? IdOrder { get; set; }
        public int? IdSchedule { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "FullName", ResourceType = typeof(LocalResources.Resource))]
        public string OrderName { get; set; }
        
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Phone", ResourceType = typeof(LocalResources.Resource))]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceName = "InputSpecialCharacter", ErrorMessageResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "OrderDate", ResourceType = typeof(LocalResources.Resource))]
        public DateTime OrderDate { get; set; }
        public string Note { get; set; }
        [Display(Name = "PickUp", ResourceType = typeof(LocalResources.Resource))]
        public string PickUp { get; set; }
        public string Destination { get; set; }
    }

    public class OrderDetailModel
    {
        public int IdOrderDetail { get; set; }
        public int IdOrder { get; set; }
        public int IdSchedule { get; set; }
        public bool IsPaid { get; set; }
        public int? SeatNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? FloorNumber { get; set; }
    }

    public class SeatChartInforModel
    {
        public int NumberFloors { get; set; }
        public List<IEnumerable<int>> SeatsList { get; set; }

        public int CurrentFloor { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "SeatNumber", ResourceType = typeof(LocalResources.Resource))]
        public int[] SeatNumbers { get; set; }

        public int IdSchedule { get; set; }

        // booked seats of customer
        public IEnumerable<OrderDetailModel> CustomerSeats { get; set; }

        // empty seats of car
        public IEnumerable<int> EmptySeats { get; set; }
        public IEnumerable<int> BookedSeatsNotPayment { get; set; }
        public IEnumerable<int> BookedSeatsPayment { get; set; }

        public SeatChartInforModel()
        {
            BookedSeatsNotPayment = Enumerable.Empty<int>();
            BookedSeatsPayment = Enumerable.Empty<int>();   
        }
    }
    public class OrderEditModel
    {
        public OrderModel order { get; set; }
        public IEnumerable<OrderDetailModel> orderDetails { get; set; }
    }
}