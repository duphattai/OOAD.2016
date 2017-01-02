using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;


namespace ServiceLayer.Service
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAll();
        Role Get(int id);
    }

    public class RoleService : IRoleService
    {
        readonly CarManagerEntities _database;
        public RoleService(CarManagerEntities db)
        {
            _database = db;
        }

        public Role Get(int id)
        {
            return _database.Roles.Find(id);
        }

        public IEnumerable<Role> GetAll()
        {
            return _database.Roles.ToList();
        }
    }
}
