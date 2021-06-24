using Accounting.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private Accounting_DBEntities _db;
        public UserRepository(Accounting_DBEntities db)
        {
            _db = db;
        }
        public bool DoesExist(string username, string password)
        {
            return _db.Login.Any(u => u.Username == username && u.Password == password);
        }

        public Login GetAdminInformation()
        {
            return _db.Login.First();
        }

        public void UpdateAdminInformation(Login login)
        {
            var admin = _db.Login.First();
            admin.Username = login.Username;
            admin.Password = login.Password;
            _db.Login.AddOrUpdate(admin);
            //_db.SaveChanges();
        }
    }
}
