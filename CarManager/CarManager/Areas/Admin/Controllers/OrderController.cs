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

        public ActionResult SeatChart(int IdSchedule, int IdOrder, int CurrentFloor = 1)
        {
            var schedule = _scheduleService.Get(IdSchedule);
            var SeatsList = new List<IEnumerable<int>>();
            var model = new SeatChartInforModel();
            
            if (schedule != null && schedule.Car.CarDiagram != null)
            {
                var carDiagram = schedule.Car.CarDiagram;

                // get floors
                model.NumberFloors = carDiagram.NumberFloors;

                // get seat chart
                var rows = carDiagram.SeatDiagram.Split('\n').Where(o => !string.IsNullOrEmpty(o));
                foreach (var r in rows)
                {
                    var seats = r.Split(' ').Select(o => int.Parse(o.Replace("x", "")));
                    SeatsList.Add(seats);
                }
                
                // get booked seats by current floor
                var currentFloorSeats = _orderDetailService.GetByScheduleID(IdSchedule, CurrentFloor);
                if (currentFloorSeats.Any())
                {
                    model.BookedSeatsPayment = currentFloorSeats.Where(t => t.IsPaid.Value).Select(o => o.SeatNumber.Value);
                    model.BookedSeatsNotPayment = currentFloorSeats.Where(t => !t.IsPaid.Value).Select(o => o.SeatNumber.Value);
                }

                // get empty seats
                var emptySeats = new List<int>();
                foreach (var item in SeatsList)
                {
                    if (currentFloorSeats.Any())
                    {
                        var except = item.Except(currentFloorSeats.Select(t => t.SeatNumber.Value));
                        if (except.Any())
                            emptySeats.AddRange(except);
                    }
                    else
                        emptySeats.AddRange(item);
                }
                model.EmptySeats = emptySeats;

                // get customer seats
                model.CustomerSeats = _mapper.Map<IEnumerable<OrderDetailModel>>(_orderDetailService.GetByOrderID(IdOrder));
            }


            model.CurrentFloor = CurrentFloor;
            model.SeatsList = SeatsList;
            model.IdSchedule = IdSchedule;
            ViewBag.IdOrder = IdOrder;

            return PartialView(model);
        }

        public ActionResult EditOrder(int id, int IdSchedule, int CurrentFloor = 1)
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
                ViewBag.CarNumberPlate = schedule.Car.CarNumberPlate;
            }
            ViewBag.CurrentFloor = CurrentFloor;

            return View(model);
        }


        [HttpPost]
        public ActionResult InsertSeat(SeatChartInforModel model, int? IdOrder)
        {
            if (ModelState.IsValid)
            {
                var bookedSeats = _orderDetailService.GetByScheduleID(model.IdSchedule, model.CurrentFloor);
                
                // check seat number existed
                bool existed = false;
                if (bookedSeats.Any())
                    existed = bookedSeats.Select(t => t.SeatNumber.Value).Intersect(model.SeatNumbers).Any();

                if (!existed) // not exist
                {
                    foreach (var seat in model.SeatNumbers)
                    {
                        var entity = new OrderDetail() 
                        {
                            FloorNumber = model.CurrentFloor,
                            IdOrder = IdOrder.Value,
                            IdSchedule = model.IdSchedule,
                            SeatNumber = seat,
                            IsPaid = false
                        };

                        string error = _orderDetailService.Insert(entity);
                    }
                }
            }
            return RedirectToAction("EditOrder", new { id = IdOrder, IdSchedule = model.IdSchedule });
        }


        public ActionResult Delete(int id, int IdOrderDetail, int IdSchedule)
        {
            string error = _orderDetailService.Delete(IdOrderDetail);

            return RedirectToAction("EditOrder", new { id = id, IdSchedule = IdSchedule });
        }

        public ActionResult Payment(int id, int[] IdOrderDetails, int IdSchedule)
        {
            _orderDetailService.UpdatePayment(IdOrderDetails);

            return RedirectToAction("EditOrder", new { id = id, IdSchedule = IdSchedule });
        }
    }
}