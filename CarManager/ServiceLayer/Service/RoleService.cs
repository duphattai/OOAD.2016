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
    }

    public class RoleService : IRoleService
    {
        readonly CarManagerEntities _database;
        public RoleService(CarManagerEntities db)
        {
            _database = db;
        }

        public IEnumerable<Role> GetAll()
        {
            return _database.Roles.ToList();
        }
    }
}
