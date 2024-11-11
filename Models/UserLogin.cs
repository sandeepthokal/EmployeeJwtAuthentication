namespace EmployeeJwtAuthentication.Models
{
    /// <summary>
    /// Model used as Request model for Login
    /// </summary>
    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
