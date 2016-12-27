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
        IEnumerable<CarDiagram> GetList();

        CarDiagram Get(int id);

        string Insert(CarDiagram entity);

        string Update(CarDiagram model);
    }
    public class CarDiagramService : ICarDiagramService
    {
        private readonly CarManagerEntities _database;
        public CarDiagramService(CarManagerEntities db)
        {
            _database = db;
        }

        public IEnumerable<CarDiagram> GetList()
        {
            return _database.CarDiagrams.ToList();
        }

        public string Insert(CarDiagram entity)
        {
            try
            {
                _database.CarDiagrams.Add(entity);
                _database.SaveChanges();

                return null;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public CarDiagram Get(int id)
        {
            return _database.CarDiagrams.Find(id);
        }

        public string Update(CarDiagram model)
        {
            try
            {
                var entity = Get(model.IdCarDiagram);
                _database.Entry(entity).CurrentValues.SetValues(model);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
