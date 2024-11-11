using EmployeeJwtAuthentication.Models;
using EmployeeJwtAuthentication.Repositories;

namespace EmployeeJwtAuthentication.Services
{
    //UserService to get the user details based on login credentials
    public class UserService : IUserService
    {
        /// <summary>
        /// GetUser method used for validating User credentials
        /// </summary>
        /// <param name="userLogin">User Login instance has username and password</param>
        /// <returns>User details on success else Not Found</returns>
        public User GetUser(UserLogin userLogin)
        {
            User user = UserRepository.Users.FirstOrDefault(u => u.Username.Equals(userLogin.Username)
                                                            && u.Password.Equals(userLogin.Password));
            return user;
        }
    }
}
