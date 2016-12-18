using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface IOrderService
    {
    }
    public class OrderService : IOrderService
    {
        private readonly CarManagerEntities _database;
        public OrderService(CarManagerEntities db)
        {
            _database = db;
        }
    }
}
