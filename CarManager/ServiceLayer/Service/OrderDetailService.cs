using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface IOrderDetailService
    {
    }
    public class OrderDetailService : IOrderDetailService
    {
        private readonly CarManagerEntities _database;
        public OrderDetailService(CarManagerEntities db)
        {
            _database = db;
        }
    }
}
