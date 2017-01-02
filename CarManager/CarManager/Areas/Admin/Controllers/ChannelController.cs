using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarManager.Areas.Admin.Models;
using ServiceLayer.Service;
using AutoMapper;
using PagedList;
using PagedList.Mvc;
using LocalResources;
using DataLayer;

namespace CarManager.Areas.Admin.Controllers
{
    public class ChannelController : BaseController
    {
        private readonly IChannelService _channelService;
        private readonly IBusStationService _busStationService;
        private readonly IMapper _mapper;
        public ChannelController(IChannelService channelService, IBusStationService busStationService, IMapper mapper)
        {
            _channelService = channelService;
            _busStationService = busStationService;
            _mapper = mapper;
        }

        // GET: Admin/Channel
        public ActionResult Index()
        {
            var busStationFrom = _channelService.GetBusStationFrom();
            if(busStationFrom != null)
                ViewBag.BusStationFroms = new SelectList(busStationFrom, "IdBusStation", "Name");

            var busStationTo = _channelService.GetBusStationTo();
            if (busStationTo != null)
                ViewBag.BusStationTos = new SelectList(busStationTo, "IdBusStation", "Name");

            return View(new ChannelFilterModel());
        }


        public PartialViewResult ChannelsList(ChannelFilterModel filter, int page = 1)
        {
            var model = _mapper.Map<IEnumerable<ChannelItemModel>>(
                _channelService.GetList(filter.BusStationFrom, filter.BusStationTo)).ToPagedList(page, _pageSize);

            foreach (var item in model)
            {
                var busList = _busStationService.GetList(item.IdBusStationFrom, item.IdBusStationTo);
                if (busList.Any())
                    item.Channel = string.Join(" - ", busList.Select(o=> o.Name));
            }

            ViewBag.BusStationFrom = filter.BusStationFrom;
            ViewBag.BusStationTo = filter.BusStationTo;

            return PartialView(model);
        }

        public ActionResult Create()
        {
            // get bus station
            var busStations = _busStationService.GetList();
            if(busStations.Any())
                ViewBag.BusStations = new SelectList(busStations, "IdBusStation", "Name");

            return View(new ChannelModel());
        }

        [HttpPost]
        public ActionResult Create(ChannelModel model, string SaveMode)
        {
            var busStations = _busStationService.GetList();
            if (busStations.Any())
                ViewBag.BusStations = new SelectList(busStations, "IdBusStation", "Name");

            if (ModelState.IsValid)
            {
                // bus station cannot the same
                if (model.IdBusStationTo == model.IdBusStationFrom)
                {
                    ModelState.AddModelError(string.Empty, Resource.ChannelTheSameError);
                    return View(model);
                }
                else
                {
                    var exist = _channelService.GetList(model.IdBusStationFrom, model.IdBusStationTo).Any();
                    if (exist) // existing channel 
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Resource.ExistingOfError, Resource.Channel));
                        return View(model);
                    }
                    else
                    {
                        var entity = _mapper.Map<Channel>(model);
                        string error = _channelService.Insert(ref entity);
                        if (error != null)
                        {
                            ModelState.AddModelError(string.Empty, error);
                            return View(model);
                        }

                        if (SaveMode == Resource.SaveAndContinueEdit)
                        {
                            return RedirectToAction("Edit", new { id = entity.IdChannel });
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, Resource.CannotInsertData);
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            // get bus station
            var busStations = _busStationService.GetList();
            if (busStations.Any())
                ViewBag.BusStations = new SelectList(busStations, "IdBusStation", "Name");

            var model = _mapper.Map<ChannelModel>(_channelService.Get(id));

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChannelModel model)
        {
            var busStations = _busStationService.GetList();
            if (busStations.Any())
                ViewBag.BusStations = new SelectList(busStations, "IdBusStation", "Name");

            if (ModelState.IsValid)
            {
                // bus station cannot the same
                if (model.IdBusStationTo == model.IdBusStationFrom)
                {
                    ModelState.AddModelError(string.Empty, Resource.ChannelTheSameError);
                    return View(model);
                }
                else
                {
                    var exist = _channelService.GetList(model.IdBusStationFrom, model.IdBusStationTo)
                        .Where(o => o.IdChannel != model.IdChannel).Any();
                    if (exist) // existing channel 
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Resource.ExistingOfError, Resource.Channel));
                        return View(model);
                    }
                    else
                    {
                        var entity = _mapper.Map<Channel>(model);
                        string error = _channelService.Update(entity);
                        if (error != null)
                        {
                            ModelState.AddModelError(string.Empty, error);
                            return View(model);
                        }

                            
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, Resource.CannotInsertData);
                return View(model);
            }
        }

        public ActionResult Delete(int id, int page = 1)
        {
            string error = _channelService.Delete(id);
            if (error != null)
            {
                return Content(null);
            }

            return RedirectToAction("ChannelsList", new { page = page });
        }
    }
}