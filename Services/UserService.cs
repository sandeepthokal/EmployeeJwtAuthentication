using EmployeeJwtAuthentication.Models;
using EmployeeJwtAuthentication.Repositories;

namespace EmployeeJwtAuthentication.Services
{
    public class UserService : IUserService
    {
        public User GetUser(UserLogin userLogin)
        {
            User user = UserRepository.Users.FirstOrDefault(u => u.Username.Equals(userLogin.Username)
                                                            && u.Password.Equals(userLogin.Password));
            return user;
        }
    }
}
