using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface IChannelDetailService
    {
    }
    public class ChannelDetailService : IChannelDetailService
    {
        private readonly CarManagerEntities _database;
        public ChannelDetailService(CarManagerEntities db)
        {
            _database = db;
        }
    }
}
