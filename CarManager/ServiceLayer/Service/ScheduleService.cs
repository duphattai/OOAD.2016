using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface IScheduleService
    {
        IEnumerable<Schedule> GetList(int? IdChannel = null, DateTime? startDate = null);

        Schedule Get(int id);

        string Insert(Schedule entity);
        string Update(Schedule entity);
        string Delete(int id);
    }
    public class ScheduleService : IScheduleService
    {
        private readonly CarManagerEntities _database;
        public ScheduleService(CarManagerEntities db)
        {
            _database = db;
        }

        public IEnumerable<Schedule> GetList(int? IdChannel = null, DateTime? startDate = null)
        {
            IEnumerable<Schedule> result;
            if (IdChannel == null)
                result = _database.Schedules.AsEnumerable();
            else
                result = _database.Schedules.Where(t => t.IdChannel == IdChannel.Value);


            if (startDate == null)
                startDate = DateTime.Now;

            result = result.Where(t => DateTime.Compare(t.StartTime.Value, startDate.Value) >= 0);

            return result;
        }

        public Schedule Get(int id)
        {
            return _database.Schedules.Find(id);
        }

        public string Insert(Schedule entity)
        {
            try
            {
                _database.Schedules.Add(entity);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(Schedule model)
        {
            try
            {
                var entity = Get(model.IdSchedule);
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
                var entity = _database.Schedules.Find(id);
                _database.Schedules.Remove(entity);
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
