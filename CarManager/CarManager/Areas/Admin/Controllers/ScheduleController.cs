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
using System.Globalization;



namespace CarManager.Areas.Admin.Controllers
{
    [CustomAuthorize("Manager")]
    public class ScheduleController : BaseController
    {
        private readonly ICarService _carService;
        private readonly ICarDiagramService _carDiagramService;
        private readonly IChannelService _channelService;
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;

        public ScheduleController(ICarService carService, ICarDiagramService carDiagramService, IChannelService channelService,
             IScheduleService scheduleService, IMapper mapper)
        {
            _carService = carService;
            _carDiagramService = carDiagramService;
            _channelService = channelService;
            _scheduleService = scheduleService;
            _mapper = mapper;
        }

        // GET: Admin/CarDiagram
        public ActionResult Index()
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

            return View(new ScheduleFilterModel());
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


        protected void SetupViewBagData()
        {
            var cars = _carService.GetList();
            if (cars.Any())
            {
                var list = cars.Select(o => new SelectListItem
                {
                    Text = o.CarNumberPlate + " / " + o.CarDiagram.Name,
                    Value = o.IdCar.ToString()
                });
                ViewBag.Cars = new SelectList(list, "Value", "Text");
            }

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

        [HttpPost]
        public JsonResult GetChannel(int IdChannel)
        {
            var channel = _channelService.Get(IdChannel);
            if (channel != null)
            {
                return Json(new { RunTime = channel.RunTime, DefaultPrice = channel.DefaultPrice});
            }
            else
                return Json(new { });
        }

        public JsonResult GetCar(int IdCar)
        {
            var car = _carService.Get(IdCar);
            if (car != null)
            {
                return Json(new { Driver = car.Driver1, TotalSeat = car.TotalSeat });
            }
            else
                return Json(new { });
        }

        public ActionResult Create()
        {
            ViewBag.IsInsert = true;
            SetupViewBagData();

            return View("InsertOrUpdate", new ScheduleModel());
        }

        [HttpPost]
        public ActionResult Create(ScheduleModel model)
        {
            ViewBag.IsInsert = true;
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Schedule>(model);
                var channel = _channelService.Get(model.IdChannel);
                if (channel != null)
                    entity.ArrivalTime = entity.StartTime.Value.AddMinutes(channel.RunTime);

                string error = _scheduleService.Insert(entity);
                if (error != null)
                {
                    SetupViewBagData();
                    ModelState.AddModelError(string.Empty, error);
                    return View("InsertOrUpdate", model);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                SetupViewBagData();
                ModelState.AddModelError(string.Empty, @Resource.CannotInsertData);
                return View("InsertOrUpdate", model);
            }
        }

        public ActionResult Edit(int id)
        {
            ViewBag.IsInsert = false;
            SetupViewBagData();

            var schedule = _scheduleService.Get(id);
            var model = _mapper.Map<ScheduleModel>(schedule);
            if(schedule != null)
            {
                model.TotalSeat = schedule.Car.TotalSeat;
                model.Driver = schedule.Car.Driver1;
            }
         

            return View("InsertOrUpdate", model);
        }

        [HttpPost]
        public ActionResult Edit(ScheduleModel model)
        {
            ViewBag.IsInsert = false;
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Schedule>(model);
                var channel = _channelService.Get(model.IdChannel);
                if (channel != null)
                    entity.ArrivalTime = entity.StartTime.Value.AddMinutes(channel.RunTime);

                string error = _scheduleService.Update(entity);
                if (error != null)
                {
                    SetupViewBagData();
                    ModelState.AddModelError(string.Empty, error);
                    return View("InsertOrUpdate", model);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                SetupViewBagData();
                ModelState.AddModelError(string.Empty, @Resource.CannotInsertData);
                return View("InsertOrUpdate", model);
            }
        }

        public ActionResult Detail(int id)
        {
            SetupViewBagData();
            var schedule = _scheduleService.Get(id);
            var model = _mapper.Map<ScheduleModel>(schedule);
            if (schedule != null)
            {
                model.TotalSeat = schedule.Car.TotalSeat;
                model.Driver = schedule.Car.Driver1;
            }

            return View(model);
        }
    }
}