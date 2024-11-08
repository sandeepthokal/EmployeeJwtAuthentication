using EmployeeJwtAuthentication.Models;

namespace EmployeeJwtAuthentication.Services
{
    public interface IUserService
    {
        User GetUser(UserLogin userLogin);
    }
}
