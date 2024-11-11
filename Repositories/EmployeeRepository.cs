using EmployeeJwtAuthentication.Models;

namespace EmployeeJwtAuthentication.Repositories
{
    /// <summary>
    /// Employee Repository manages the Data for Employee Model
    /// </summary>
    public class EmployeeRepository
    {
        /// <summary>
        /// Mocked the data for Employees which will be used for CRUD operations
        /// </summary>
        public static List<Employee> Employees = new()
        {
            new Employee() { Id =1, Name="Deepak Thokal", Email="deepak.thokal@email.com", City="Mumbai",
                            Country = "India", Designation = "Software Engineer"},
            new Employee() { Id =2, Name="Rohit Sharma", Email="rohit.sharma@email.com", City="Mumbai",
                            Country = "India", Designation = "Senior Software Engineer"},
            new Employee() { Id =3, Name="Virat Kohli", Email="virat.kohli@email.com", City="Mumbai",
                            Country = "India", Designation = "Lead Consultant"},
            new Employee() { Id =4, Name="Gautam Gambhir", Email="gautam.gambhie@email.com", City="Mumbai",
                            Country = "India", Designation = "Delivery Manager"},
            new Employee() { Id =5, Name="Sachin Tendulkar", Email="sachin.tendulkar@email.com", City="Mumbai",
                            Country = "India", Designation = "Business Analyst"},
            new Employee() { Id =6, Name="Rahul Dravid", Email="rahul.dravid@email.com", City="Mumbai",
                            Country = "India", Designation = "Head Of Department"},
        };
    }
}
