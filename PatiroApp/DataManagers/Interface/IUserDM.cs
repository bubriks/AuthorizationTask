using PatiroApp.Models;
using System.Collections.Generic;

namespace PatiroApp.DataManagers.Interface
{
    public interface IUserDM
    {
        User Authenticate(string username);
        List<User> GetUsers();
    }
}
