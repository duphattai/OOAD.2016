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

        IEnumerable<BusStation> GetBusStationFrom();
        IEnumerable<BusStation> GetBusStationTo();
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

        public IEnumerable<BusStation> GetBusStationFrom()
        {
            var busStationFrom = _database.Channels
                .Select(o=> o.IdBusStationFrom).Distinct().ToList(); // make distinct

            if (busStationFrom.Count() != 0)
                return _database.BusStations.Where(t => busStationFrom.Contains(t.IdBusStation));
            else 
                return null;
        }

        public IEnumerable<BusStation> GetBusStationTo()
        {
            var busStationTo = _database.Channels.Select(o=> o.IdBusStationTo).Distinct().ToList(); // make distinct


            if (busStationTo.Count() != 0)
                return _database.BusStations.Where(t => busStationTo.Contains(t.IdBusStation));
            else
                return null;
        }
    }
}
