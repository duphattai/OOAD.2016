using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface IBusStationService
    {
    }
    public class BusStationService : IBusStationService
    {
        private readonly CarManagerEntities _database;
        public BusStationService(CarManagerEntities db)
        {
            _database = db;
        }
    }
}
