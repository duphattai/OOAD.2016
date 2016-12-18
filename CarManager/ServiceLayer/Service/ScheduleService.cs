using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface IScheduleService
    {
    }
    public class ScheduleService : IScheduleService
    {
        private readonly CarManagerEntities _database;
        public ScheduleService(CarManagerEntities db)
        {
            _database = db;
        }
    }
}
