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
        IEnumerable<BusStation> GetList(string SearchString = null);

        IEnumerable<BusStation> GetList(params int[] id);

        BusStation Get(int id);

        string Insert(BusStation entity);
        string Update(BusStation entity);
        string Delete(int id);
    }
    public class BusStationService : IBusStationService
    {
        private readonly CarManagerEntities _database;
        public BusStationService(CarManagerEntities db)
        {
            _database = db;
        }

        public BusStation Get(int id)
        {
            return _database.BusStations.Find(id);
        }

        public IEnumerable<BusStation> GetList(string SearchString = null)
        {
            if (string.IsNullOrEmpty(SearchString))
                return _database.BusStations.ToList();
            else
            {
                return _database.BusStations.Where(o => o.Name.ToLower().Contains(SearchString.Trim().ToLower()));
            }

        }

        public IEnumerable<BusStation> GetList(params int[] id)
        {
            return _database.BusStations.Where(t => id.Contains(t.IdBusStation));
        }

        public string Insert(BusStation entity)
        {
            try 
            {
                entity.Name = entity.Name.Trim();

                _database.BusStations.Add(entity);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(BusStation model)
        {
            try
            {
                var entity = Get(model.IdBusStation);
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
                var entity = _database.BusStations.Find(id);
                _database.BusStations.Remove(entity);
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
