using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Repositories
{
    public interface IUserRepository
    {
        bool DoesExist(string username, string password);
        Login GetAdminInformation();

        void UpdateAdminInformation(Login login);
    }
}
