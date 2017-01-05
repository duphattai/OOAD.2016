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
        IEnumerable<Order> GetList(int? IdSchedule = null, string SearchString = null, string Phone = null);

        string Insert(Order entity);
        string Update(Order entity);
        string Delete(int id);

        Order Get(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly CarManagerEntities _database;
        public OrderService(CarManagerEntities db)
        {
            _database = db;
        }

        public string Insert(Order entity)
        {
            try
            {
                entity.OrderDate = DateTime.Now;
                _database.Orders.Add(entity);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Order Get(int id)
        {
            return _database.Orders.Find(id);
        }


        public string Update(Order model)
        {
            try
            {
                var entity = Get(model.IdOrder);
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
                var entity = _database.Orders.Find(id);
                _database.Orders.Remove(entity);
                _database.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        public IEnumerable<Order> GetList(int? IdSchedule = null, string SearchString = null, string Phone = null)
        {
            // -------------- Get order detail ------------------------
            IEnumerable<OrderDetail> orderDetails;
            if (IdSchedule != null)
                orderDetails = _database.OrderDetails.Where(t => t.IdSchedule == IdSchedule.Value);
            else
                orderDetails = _database.OrderDetails.AsEnumerable();
            if (!orderDetails.Any()) 
                return Enumerable.Empty<Order>();

            // ----------- Get order -------------
            // from name
            IEnumerable<int> orderIDs = orderDetails.Select(t => t.IdOrder).Distinct();
            IEnumerable<Order> orders;
            if (!string.IsNullOrEmpty(SearchString))
                orders = _database.Orders.Where(t => t.OrderName.ToLower().Contains(SearchString.ToLower()) && orderIDs.Contains(t.IdOrder));
            else
                orders = _database.Orders.Where(t => orderIDs.Contains(t.IdOrder));
            if (!orders.Any())
                return Enumerable.Empty<Order>();

            // from phone number
            if (!string.IsNullOrEmpty(SearchString))
                orders = orders.Where(t => t.PhoneNumber.ToLower().Contains(Phone.ToLower()));
         
            return orders;
        }
    }
}
