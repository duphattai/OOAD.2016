using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer.Service
{
    public interface IAccountService
    {
    }
    public class AccountService : IAccountService
    {
        private readonly CarManagerEntities _database;
        public AccountService(CarManagerEntities db)
        {
            _database = db;
        }
    }
}
