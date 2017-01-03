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
    public class CarDiagramController : BaseController
    {
        private readonly ICarDiagramService _carDiagramService;
        private readonly IMapper _mapper;

        public CarDiagramController(ICarDiagramService carDiagramService, IMapper mapper)
        {
            _carDiagramService = carDiagramService;
            _mapper = mapper;
        }

        // GET: Admin/CarDiagram
        public ActionResult Index()
        {
            return View();
        }

           
        public PartialViewResult CarDiagramsList(int page = 1)
        {
            var model = _mapper.Map<IEnumerable<CarDiagramItemModel>>(_carDiagramService.GetList())
                            .ToPagedList(page, _pageSize);
            return PartialView(model);
        }

        public ActionResult Create()
        {
            return View(new CarDiagramModel());
        }

        [HttpPost]
        public ActionResult Create(CarDiagramModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<CarDiagram>(model);
                string errorInsert = _carDiagramService.Insert(entity);

                if(errorInsert != null)
                {
                    ModelState.AddModelError(string.Empty, errorInsert);
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, LocalResources.Resource.CannotInsertData);
                return View(model);
            }
        }


        public ActionResult Edit(int id)
        {
            var model = _mapper.Map<CarDiagramModel>(_carDiagramService.Get(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CarDiagramModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<CarDiagram>(model);
                string error = _carDiagramService.Update(entity);

                if (error != null)
                {
                    ModelState.AddModelError(string.Empty, error);
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, LocalResources.Resource.CannotInsertData);
                return View(model);
            }
        }


        public ActionResult Detail(int id)
        {
            var model = _mapper.Map<CarDiagramModel>(_carDiagramService.Get(id));
            return View(model);
        }


        public ActionResult Delete(int id, int page = 1)
        {
            string error = _carDiagramService.Delete(id);
            if (error != null)
            {
                return Content(null);
            }

            return RedirectToAction("CarDiagramsList", new { page = page });
        }
    }
}