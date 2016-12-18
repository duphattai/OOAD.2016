using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface ICarDiagramService
    {
    }
    public class CarDiagramService : ICarDiagramService
    {
        private readonly CarManagerEntities _database;
        public CarDiagramService(CarManagerEntities db)
        {
            _database = db;
        }
    }
}
