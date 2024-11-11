using EmployeeJwtAuthentication.Models;

namespace EmployeeJwtAuthentication.Repositories
{
    //UserRepository manages the data for User entity
    public class UserRepository
    {
        /// <summary>
        /// Mocked the data for Users which will be used further in login
        /// </summary>
        public static List<User> Users = new()
        {
            new()
            {
                Username = "harbinger_admin",
                Password = "@dmin$ecureP@ssword",
                Email = "harbinger.admin@email.com",
                FirstName = "Sandeep",
                LastName = "Thokal",
                Role = "Administrator"
            },
            new()
            {
                Username = "harbinger_member",
                Password = "My$ecureP@ssword",
                Email = "harbinger.admin@email.com",
                FirstName = "Ravi",
                LastName = "Thokal",
                Role = "Member"
            }
        };
    }
}
