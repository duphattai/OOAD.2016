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
                var seats = _orderDetailService.GetByOrderID(item.IdOrder).Select(o => o.SeatNumber);
                if (seats.Any())
                    item.Seats = string.Join(", ", seats);
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

        public ActionResult Create()
        {
            return View();
        }
    }
}