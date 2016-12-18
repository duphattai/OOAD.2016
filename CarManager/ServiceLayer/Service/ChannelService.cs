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
    }
    public class ChannelService : IChannelService
    {
        private readonly CarManagerEntities _database;
        public ChannelService(CarManagerEntities db)
        {
            _database = db;
        }
    }
}
