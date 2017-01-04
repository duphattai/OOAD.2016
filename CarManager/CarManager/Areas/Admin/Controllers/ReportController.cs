using CarManager.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.Service;
using AutoMapper;
using PagedList;
using DataLayer;

namespace CarManager.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;       
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;            
            _mapper = mapper;
        }

        public ActionResult ReportByMonth()
        {
            var report = new ReportModel();
            var Month = DateTime.Now.Month;
            var Year = DateTime.Now.Year;
            var listM = _reportService.GetListMonth();
            var itemsM = listM.Select(month => new SelectListItem
            {
                Text = "Tháng " +  month.ToString(),
                Value = month.ToString()
            });
            ViewBag.Months = itemsM;
            
            var listY   = _reportService.GetListYear();
            var itemsY = listY.Select(year => new SelectListItem
            {
                Text = "Năm " + year.ToString(),
                Value = year.ToString()
            });
            ViewBag.Years = itemsY;

            var result = _reportService.ReportByMonth(Month, Year);
            var model = _mapper.Map<IEnumerable<ReportModel>>(result);

            ViewBag.Lists = model;

            report.Month = Month;
            report.Year = Year;
            report.TotalTicked = model.Sum(t => t.TOTAL_TICKED);
            report.TotalPrice = model.Sum(t => t.TOTAL_PRICE); 

            return View(report);
        }

        [HttpPost]
        public ActionResult ReportByMonth(ReportModel rp)
        {
            var result = new List<ReportByDate_Result>();
            int month = rp.Month;
            int year = rp.Year;

            if (month == 0 || year == 0)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            
            result = _reportService.ReportByMonth(month, year);
            
            var model = _mapper.Map<IEnumerable<ReportModel>>(result);

            foreach (var item in model)
            {
                item.Month = month;
                item.Year = year;
                item.TotalTicked = model.Sum(t=>t.TOTAL_TICKED);
                item.TotalPrice = model.Sum(t=>t.TOTAL_PRICE);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportByYear()
        {
            var report = new ReportModel();
            var Year = DateTime.Now.Year;
            
            var listY = _reportService.GetListYear();
            var itemsY = listY.Select(year => new SelectListItem
            {
                Text = "Năm " + year.ToString(),
                Value = year.ToString()
            });
            ViewBag.Years = itemsY;

            var result = _reportService.ReportByYear(Year);
            var model = _mapper.Map<IEnumerable<ReportModel>>(result);

            ViewBag.Lists = model;
            
            report.Year = Year;
            report.TotalTicked = model.Sum(t => t.TOTAL_TICKED);
            report.TotalPrice = model.Sum(t => t.TOTAL_PRICE);

            return View(report);
        }

        [HttpPost]
        public ActionResult ReportByYear(ReportModel rp)
        {
            var result = new List<ReportByDate_Result>();            
            int year = rp.Year;

            if (year == 0)
            {                
                year = DateTime.Now.Year;
            }

            result = _reportService.ReportByYear(year);

            var model = _mapper.Map<IEnumerable<ReportModel>>(result);

            foreach (var item in model)
            {
                item.Year = year;
                item.TotalTicked = model.Sum(t => t.TOTAL_TICKED);
                item.TotalPrice = model.Sum(t => t.TOTAL_PRICE);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}