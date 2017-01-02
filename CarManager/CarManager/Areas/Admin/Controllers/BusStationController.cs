using AutoMapper;
using CarManager.Areas.Admin.Models;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using DataLayer;
using LocalResources;

namespace CarManager.Areas.Admin.Controllers
{
    public class BusStationController : BaseController
    {
        private readonly IBusStationService _busStationService;
        private readonly IMapper _mapper;

        public BusStationController(IBusStationService busStationService, IMapper mapper)
        {
            _busStationService = busStationService;
            _mapper = mapper;
        }

        // GET: Admin/Channel
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult BusStationsList(string SearchString, int page = 1)
        {
            var model = _mapper.Map<IEnumerable<BusStationModel>>(_busStationService.GetList(SearchString)).ToPagedList(page,_pageSize);
            ViewBag.SearchString = SearchString;
            return PartialView(model);
        }

      

        public ActionResult Edit(int id)
        {
            ViewBag.IsInsert = false;
            var model = _mapper.Map<BusStationModel>(_busStationService.Get(id));
            return View("InsertOrUpdate", model);           
        }
        [HttpPost]
        public ActionResult Edit(BusStationModel model)
        {
            ViewBag.IsInsert = false;
            // does existing bus station
            var result = _busStationService.GetList(null).Where(o => o.IdBusStation != model.IdBusStation && o.Name == model.Name);
            if (result.Count() == 0) // not existing, can insert
            {
                var entity = _mapper.Map<BusStation>(model);
                string error = _busStationService.Update(entity);
                if (error != null)
                {
                    ModelState.AddModelError(string.Empty, error);
                    return View("InsertOrUpdate", model);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format(Resource.ExistingNameOfError, Resource.BusStation));
                return View("InsertOrUpdate", model);
            }
        }

        public ActionResult Create()
        {
            ViewBag.IsInsert = true;
            return View("InsertOrUpdate", new BusStationModel());
        }


        [HttpPost]
        public ActionResult Create(BusStationModel model)
        {
            ViewBag.IsInsert = true;
            // does existing bus station
            if (_busStationService.GetList(null).Where(o=> o.Name.ToLower() == model.Name.Trim().ToLower()).Count() == 0) // not existing, can insert
            {
                var entity = _mapper.Map<BusStation>(model);
                string error = _busStationService.Insert(entity);
                if (error != null)
                {
                     ModelState.AddModelError(null, error);
                    return View("InsertOrUpdate", model);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format(Resource.ExistingNameOfError, Resource.BusStation));
                return View("InsertOrUpdate", model);
            }    
        }

        public ActionResult Delete(int id, int page = 1)
        {
            string error = _busStationService.Delete(id);
            if (error != null)
            {
                return Content(null);
            }

            return RedirectToAction("BusStationsList", new { page = page });
        }
    }
}