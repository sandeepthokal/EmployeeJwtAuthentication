using EmployeeJwtAuthentication.Models;

namespace EmployeeJwtAuthentication.Services
{
    //Interface for User service which will return User details
    public interface IUserService
    {
        /// <summary>
        /// Get User details based on the login credentials
        /// </summary>
        /// <param name="userLogin">User login contains username and password</param>
        /// <returns>User details</returns>
        User GetUser(UserLogin userLogin);
    }
}
