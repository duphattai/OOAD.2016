using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface IChannelService
    {
        IEnumerable<Channel> GetList(int? BusStationFrom = null, int? BusStationTo = null);

        List<BusStation> GetBusStationFrom();
        List<BusStation> GetBusStationTo();

        Channel Get(int id);

        string Insert(ref Channel entity);
        string Update(Channel entity);
        string Delete(int id);
    }
    public class ChannelService : IChannelService
    {
        private readonly CarManagerEntities _database;
        public ChannelService(CarManagerEntities db)
        {
            _database = db;
        }

        public IEnumerable<Channel> GetList(int? BusStationFrom = null, int? BusStationTo = null)
        {
            var result = _database.Channels.AsEnumerable();

            if (BusStationFrom != null)
                result = result.Where(t => t.IdBusStationFrom == BusStationFrom.Value);

            if(BusStationTo != null)
                result = result.Where(t => t.IdBusStationTo == BusStationTo.Value);

            return result;
        }

        public List<BusStation> GetBusStationFrom()
        {
            var busStationFrom = _database.Channels
                .Select(o=> o.IdBusStationFrom).Distinct().ToList(); // make distinct

            if (busStationFrom.Count() != 0)
                return _database.BusStations.Where(t => busStationFrom.Contains(t.IdBusStation)).ToList();
            else 
                return null;
        }

        public List<BusStation> GetBusStationTo()
        {
            var busStationTo = _database.Channels
                .Select(o=> o.IdBusStationTo).Distinct().ToList(); // make distinct


            if (busStationTo.Count() != 0)
                return _database.BusStations.Where(t => busStationTo.Contains(t.IdBusStation)).ToList();
            else
                return null;
        }

        public string Insert(ref Channel entity)
        {
            try
            {
                _database.Channels.Add(entity);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Channel Get(int id)
        {
            return _database.Channels.Find(id);
        }

        public string Update(Channel model)
        {
            try
            {
                var entity = Get(model.IdChannel);
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
                var entity = Get(id);
                _database.Channels.Remove(entity);
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
