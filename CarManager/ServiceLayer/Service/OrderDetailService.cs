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
        IEnumerable<OrderDetail> GetByOrderID(int id);
    }
    public class OrderDetailService : IOrderDetailService
    {
        private readonly CarManagerEntities _database;
        public OrderDetailService(CarManagerEntities db)
        {
            _database = db;
        }

        public IEnumerable<OrderDetail> GetByOrderID(int id)
        {
            return _database.OrderDetails.Where(t => t.IdOrder == id);
        }
    }
}
