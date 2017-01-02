using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Security.Cryptography;

namespace ServiceLayer.Service
{
    public interface IAccountService
    {
        Account DoesAccountExist(string userName, string pass);

        string Insert(Account entity);
        string Update(Account entity);
        string Delete(int id);

        Account Get(int id);
        IEnumerable<Account> GetList(int? idRole = null);

        bool DoesUserNameExist(string username);
        bool PasswordNotMatch(string pass);
    }
    public class AccountService : IAccountService
    {
        private readonly CarManagerEntities _database;
        public AccountService(CarManagerEntities db)
        {
            _database = db;
        }



        #region Md5 hash
        protected string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        protected bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        public Account DoesAccountExist(string userName, string pass)
        {
            MD5 md5Hash = MD5.Create();
            pass = GetMd5Hash(md5Hash, pass);

            var account = _database.Accounts.Where(t => t.UserName == userName.Trim() 
                && pass == t.Pass).FirstOrDefault();

            return account;
        }

        public string Insert(Account entity)
        {
            try
            {
                MD5 md5Hash = MD5.Create();
                entity.Pass = GetMd5Hash(md5Hash, entity.Pass);
                _database.Accounts.Add(entity);
                _database.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(Account model)
        {
            try
            {
                var entity = Get(model.IdAccount);
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
                var entity = _database.Accounts.Find(id);
                _database.Accounts.Remove(entity);
                _database.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Account Get(int id)
        {
            return _database.Accounts.Find(id);
        }

        public IEnumerable<Account> GetList(int? idRole = null)
        {
            if (idRole == null)
                return _database.Accounts.ToList();
            else
            {
                var result = _database.Accounts.ToList();
                result = result.Where(item => item.IdRoles.Split(',').Select(o => Int32.Parse(o)).ToArray().Contains(idRole.Value)).ToList();
                //foreach(var item in result)
                //{
                //    int[] roles = item.IdRoles.Split(',').Select(o => Int32.Parse(o)).ToArray();
                //    if (!roles.Contains(idRole.Value))
                //        result.Remove(item);
                //}

                return result;
            }
        }

        public bool DoesUserNameExist(string username)
        {
            bool check = false;
            var accounts = _database.Accounts.ToList();

            foreach (var item in accounts)
            {
                if (item.UserName == username)
                {
                    check = true;
                    break;
                }                
            }

            return check;
        }

        public bool PasswordNotMatch(string pass)
        {
            throw new NotImplementedException();
        }
    }
}
