using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.Service;
using AutoMapper;
using CarManager.Areas.Admin.Models;
using PagedList.Mvc;
using PagedList;
using DataLayer;
using CarManager.Infrastructure.Attributes;
using LocalResources;



namespace CarManager.Areas.Admin.Controllers
{
    [CustomAuthorize("Manager")]
    public class OrderController : BaseController
    {
        private readonly ICarService _carService;
        private readonly IScheduleService _scheduleService;
        private readonly IChannelService _channelService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;

        public OrderController(ICarService carService, IChannelService channelService,
            IOrderService orderService, IOrderDetailService orderDetailService, IScheduleService scheduleService, 
            IMapper mapper)
        {
            _carService = carService;
            _scheduleService = scheduleService;
            _channelService = channelService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
        }


        protected void SetupViewBagData()
        {
            var channels = _channelService.GetList();
            if (channels.Any())
            {
                var list = channels.Select(o => new SelectListItem
                {
                    Text = o.BusStationFrom.Name + " - " + o.BusStationTo.Name,
                    Value = o.IdChannel.ToString()
                });
                ViewBag.Channels = new SelectList(list, "Value", "Text");
            }
        }

        

        #region ticket search
        public ActionResult TicketSearch()
        {
            SetupViewBagData();
            return View();
        }

        public PartialViewResult OrdersList(OrderFilterModel filter, int page = 1)
        {
            // get schedules from channel, start date
            var schedules = _scheduleService.GetList(filter.IdChannel, filter.StartDate);

            // get orders
            List<Order> orders = new List<Order>();
            foreach (var item in schedules)
            {
                var ord = _orderService.GetList(item.IdSchedule, filter.SearchString, filter.Phone);
                if (ord.Any())
                    orders.AddRange(ord);
            }

            // setup data
            var model = _mapper.Map<IEnumerable<OrderItemModel>>(orders).ToPagedList(page, _pageSize);
            foreach (var item in model)
            {
                var seats = _orderDetailService.GetByOrderID(item.IdOrder);
                if (seats.Any())
                {
                    item.Seats = string.Join(", ", seats.Select(o => o.SeatNumber));
                    item.IdSchedule = seats.Select(o => o.IdSchedule).First();
                }
            }

            // store filter 
            ViewBag.SearchString = filter.SearchString;
            ViewBag.Phone = filter.Phone;
            ViewBag.StartDate = filter.StartDate;
            ViewBag.IdChannel = filter.IdChannel;

            return PartialView(model);
        }

        #endregion


        #region schedule search
        // GET: Admin/CarDiagram
        public ActionResult Index()
        {
            SetupViewBagData();
            return View();
        }

        public PartialViewResult SchedulesList(ScheduleFilterModel filter, int page = 1)
        {
            if (ModelState.IsValid)
            {
                DateTime? startDate = null;
                if (!string.IsNullOrEmpty(filter.StartDate))
                    startDate = DateTime.Parse(filter.StartDate);

                ViewBag.IdChannel = filter.IdChannel;
                ViewBag.StartDate = filter.StartDate;

                var model = _mapper.Map<IEnumerable<ScheduleItemModel>>(_scheduleService.GetList(filter.IdChannel, startDate)).ToPagedList(page, _pageSize);
                return PartialView(model);
            }
            else
                return PartialView();
        }
        #endregion

        public ActionResult Create(int IdSchedule)
        {
            var model = new OrderModel() 
            { 
                IdSchedule = IdSchedule,
                OrderDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Order>(model);
                entity.OrderDate = DateTime.Now;

                string error = _orderService.Insert(entity);
                if (error != null)
                {
                    ModelState.AddModelError(string.Empty, error);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("EditOrder", new { IdSchedule = model.IdSchedule, id = entity.IdOrder });
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, Resource.CannotInsertData);
                return View(model);
            }
        }

        public ActionResult SeatChart(int IdSchedule, int IdOrder, int? CurrentFloor = null)
        {
            var schedule = _scheduleService.Get(IdSchedule);
            var SeatsList = new List<IEnumerable<string>>();
            var model = new SeatChartModel();
            
            if (schedule != null)
            {
                var carDiagram = schedule.Car.CarDiagram;
                if (carDiagram != null)
                {
                    // get floors
                    model.NumberFloors = carDiagram.NumberFloors;
                    CurrentFloor = CurrentFloor != null ? CurrentFloor : 1; // default current floor 1

                    // get seat chart
                    var rows = carDiagram.SeatDiagram.Split('\n').Where(o => !string.IsNullOrEmpty(o));
                    foreach (var r in rows)
                    {
                        var seats = r.Split(' ').Select(o => o.Replace("x", ""));
                        SeatsList.Add(seats);
                    }
                }
            }


            model.CurrentFloor = CurrentFloor.Value;
            model.SeatsList = SeatsList;
            ViewBag.IdSchedule = IdSchedule;
            ViewBag.IdOrder = IdOrder;

            return PartialView(model);
        }

        public ActionResult EditOrder(int id, int IdSchedule)
        {
            var model = new OrderEditModel();
            // customer information
            model.order = _mapper.Map<OrderModel>(_orderService.Get(id));
            model.order.IdSchedule = IdSchedule;


            // get information schedule
            var schedule = _scheduleService.Get(IdSchedule);
            if (schedule != null)
            {
                ViewBag.Channel = schedule.Channel.BusStationFrom.Name + " - " +schedule.Channel.BusStationTo.Name;
                ViewBag.StartTime = schedule.StartTime.Value.ToString(Resource.DateTimeFormat);
                ViewBag.Price = schedule.Price;
            }
            
            return View(model);
        }


        [HttpPost]
        public ActionResult InsertSeat(int[] SeatNumbers, int? IdSchedule, int? IdOrder)
        {
            if (ModelState.IsValid)
            {
                
            }
            return RedirectToAction("EditOrder", new { id = IdOrder, IdSchedule = IdSchedule });
        }
    }
}