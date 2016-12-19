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
    }
}
