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
        IEnumerable<OrderDetail> GetByScheduleID(int IdSchedule, int Floor);

        string Insert(OrderDetail entity);
        string DeleteByOrderID(int orderID);

        string UpdatePayment(int[] Ids);

        string Delete(int id);
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

        public IEnumerable<OrderDetail> GetByScheduleID(int IdSchedule, int Floor)
        {
            return _database.OrderDetails.Where(t => t.IdSchedule == IdSchedule && t.FloorNumber == Floor);
        }

        public string Insert(OrderDetail entity)
        {
            try
            {
                _database.OrderDetails.Add(entity);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeleteByOrderID(int orderID)
        {
            try
            {
                var orderDetails = _database.OrderDetails.Where(t => t.IdOrder == orderID);
                if(orderDetails.Any())
                {
                    _database.OrderDetails.RemoveRange(orderDetails);
                    _database.SaveChanges();
                }
                   
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
                var entity = _database.OrderDetails.Find(id);

                _database.OrderDetails.Remove(entity);
                _database.SaveChanges();
               
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdatePayment(int[] Ids)
        {
            try 
            {
                var orderDetails = _database.OrderDetails.Where(t => Ids.Contains(t.IdOrderDetail));
                foreach (var item in orderDetails)
                    item.IsPaid = true;

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
