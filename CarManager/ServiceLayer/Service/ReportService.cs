using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Security.Cryptography;

namespace ServiceLayer.Service
{
    public interface IReportService
    {
        List<ReportByDate_Result> ReportByMonth(int month, int year);
        List<ReportByDate_Result> ReportByYear(int year);
        List<int> GetListMonth();
        List<int> GetListYear();
    }
    public class ReportService : IReportService
    {
        private readonly CarManagerEntities _database;
        public ReportService(CarManagerEntities db)
        {
            _database = db;
        }

        public List<int> GetListMonth()
        {
            List<int> month = new List<int>();

            for (int i = 1; i < 13; i++)
            {
                month.Add(i);
            }

            return month;
        }

        public List<int> GetListYear()
        {
            List<int> year = new List<int>();

            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                year.Add(i);
            }

            return year;
        }

        public List<ReportByDate_Result> ReportByMonth(int month, int year)
        {
            ReportByDate_Result rp = new ReportByDate_Result();
            var list = new List<ReportByDate_Result>();
            int count = DateTime.DaysInMonth(year, month);
            string date = "";
            
            for (int i = 1; i <= count; i++)
            {
                if (month<10)
                {
                    if (i < 10)
                        date = "0" + i + "/0" + month + "/" + year;
                    else
                        date = i + "/0" + month + "/" + year;

                    rp =_database.ReportByDate(date).SingleOrDefault();
                    list.Add(rp);
                }
                else
                {
                    if (i < 10)
                        date = "0" + i + "/" + month + "/" + year;
                    else
                        date = i + "/" + month + "/" + year;

                    rp = _database.ReportByDate(date).SingleOrDefault();
                    list.Add(rp);
                }
                
            }

            return list;
        }

        public List<ReportByDate_Result> ReportByYear(int year)
        {           
            var listMonth = new List<ReportByDate_Result>();
            var listYear = new List<ReportByDate_Result>();
            for (int i = 1; i < 13; i++)
            {
                var rp = new ReportByDate_Result();
                listMonth = ReportByMonth(i, year);

                rp.TOTAL_TICKED = listMonth.Sum(t => t.TOTAL_TICKED);
                rp.TOTAL_PRICE = listMonth.Sum(t => t.TOTAL_PRICE);

                listYear.Add(rp);
            }

            return listYear;
        }
    }
}
