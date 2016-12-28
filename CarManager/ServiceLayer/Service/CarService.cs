using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface ICarService
    {
        string Insert(Car entity);
        string Update(Car entity);
        string Delete(int id);

        Car Get(int id);
        IEnumerable<Car> GetList(int? idCarDiagram = null);
    }
    public class CarService : ICarService
    {
        private readonly CarManagerEntities _database;
        public CarService(CarManagerEntities db)
        {
            _database = db;
        }

        public string Insert(Car entity)
        {
            try
            {
                _database.Cars.Add(entity);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Car Get(int id)
        {
            return _database.Cars.Find(id);
        }

        public string Update(Car model)
        {
            try
            {
                var entity = Get(model.IdCar);
                _database.Entry(entity).CurrentValues.SetValues(model);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                var entity = _database.Cars.Find(id);
                _database.Cars.Remove(entity);

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IEnumerable<Car> GetList(int? idCarDiagram = null)
        {
            if (idCarDiagram == null)
                return _database.Cars.ToList();
            else
                return _database.Cars.Where(t => t.IdCarDiagram == idCarDiagram.Value);
        }
    }
}
