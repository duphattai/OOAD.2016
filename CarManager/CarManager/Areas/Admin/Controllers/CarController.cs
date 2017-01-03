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
    public class CarController : BaseController
    {
        private readonly ICarService _carService;
        private readonly ICarDiagramService _carDiagramService;
        private readonly IMapper _mapper;

        public CarController(ICarService carService, ICarDiagramService carDiagramService, IMapper mapper)
        {
            _carService = carService;
            _carDiagramService = carDiagramService;
            _mapper = mapper;
        }

        // GET: Admin/CarDiagram
        public ActionResult Index()
        {
            var carDiagrams = _carDiagramService.GetList();
            ViewBag.CarDiagrams = new SelectList(carDiagrams, "IdCarDiagram", "Name");

            return View(new FilterCarModel());
        }

        [HttpGet]
        public PartialViewResult CarsList(FilterCarModel filter, int page = 1)
        {
            var model = _mapper.Map<IEnumerable<CarItemModel>>(_carService.GetList(filter.IdCarDiagram));
            model = model.ToPagedList(page, _pageSize);

            foreach (var item in model)
            {
                var carDiagram = _carDiagramService.Get(item.IdCarDiagram);
                if (carDiagram != null)
                {
                    item.CarDiagramName = carDiagram.Name;
                    item.TypeSeat = carDiagram.TypeSeat;
                }
            }
            ViewBag.IdCarDiagram = filter.IdCarDiagram;

            return PartialView(model);
        }

        public ActionResult SeatChart(int? IdCarDiagram = null)
        {
            var model = new SeatChartModel() { TotalSeat = 0 };
            if (IdCarDiagram != null)
            {
                var carDiagram = _carDiagramService.Get(IdCarDiagram.Value);
                if (carDiagram != null)
                {
                    model.NumberFloors = carDiagram.NumberFloors;

                    var rows = carDiagram.SeatDiagram.Split('\n').Where(o => !string.IsNullOrEmpty(o));
            
                    foreach (var r in rows)
                    {
                        var seats = r.Split(' ').Select(o=> o.Replace("x", ""));
                        model.SeatsList.Add(seats);

                        model.TotalSeat += seats.Count();
                    }
                    model.TotalSeat *= model.NumberFloors;
                }
            }

            return PartialView("SeatChart", model);
        }

        public ActionResult Create()
        {
            var carDiagrams = _carDiagramService.GetList();
            ViewBag.CarDiagrams = new SelectList(carDiagrams, "IdCarDiagram", "Name");
            
            ViewBag.IsInsert = true;
            return View("InsertOrUpdate",new CarModel());
        }

        [HttpPost]
        public ActionResult Create(CarModel model)
        {
            ViewBag.CarDiagrams = new SelectList(_carDiagramService.GetList(), "IdCarDiagram", "Name");
            ViewBag.IsInsert = true;
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Car>(model);
                string errorInsert = _carService.Insert(entity);

                if (errorInsert != null)
                {
                    ModelState.AddModelError(string.Empty, errorInsert);
                    return View("InsertOrUpdate", model);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, LocalResources.Resource.CannotInsertData);
                return View("InsertOrUpdate", model);
            }
        }


        public ActionResult Edit(int id)
        {
            ViewBag.CarDiagrams = new SelectList(_carDiagramService.GetList(), "IdCarDiagram", "Name");
            ViewBag.IsInsert = false;
            var model = _mapper.Map<CarModel>(_carService.Get(id));
            return View("InsertOrUpdate", model);
        }

        [HttpPost]
        public ActionResult Edit(CarModel model)
        {
            ViewBag.CarDiagrams = new SelectList(_carDiagramService.GetList(), "IdCarDiagram", "Name");
            ViewBag.IsInsert = false;
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Car>(model);
                string error = _carService.Update(entity);

                if (error != null)
                {
                    ModelState.AddModelError(string.Empty, error);
                    return View("InsertOrUpdate", model);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, LocalResources.Resource.CannotInsertData);
                return View("InsertOrUpdate", model);
            }
        }


        public ActionResult Detail(int id)
        {
            ViewBag.CarDiagrams = new SelectList(_carDiagramService.GetList(), "IdCarDiagram", "Name");
            var model = _mapper.Map<CarModel>(_carService.Get(id));
            return View(model);
        }

        public ActionResult Delete(int id, int page = 1)
        {
            string error = _carService.Delete(id);
            if (error != null)
            {
                return Content(null);
            }

            return RedirectToAction("CarsList", new { page = page });
        }
    }
}