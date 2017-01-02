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

namespace CarManager.Areas.Admin.Controllers
{
    public class ChannelController : BaseController
    {
        private readonly IChannelService _channelService;
        private readonly IMapper _mapper;
        public ChannelController(IChannelService channelService, IMapper mapper)
        {
            _channelService = channelService;
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

            ViewBag.BusStationFrom = filter.BusStationFrom;
            ViewBag.BusStationTo = filter.BusStationTo;

            return PartialView(model);
        }
    }
}