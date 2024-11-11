using EmployeeJwtAuthentication.Models;

namespace EmployeeJwtAuthentication.Services
{
    //This interface has declaration of CRUD operations on Employee
    public interface IEmployeeService
    {
        public Employee AddEmployee(Employee employee);
        public Employee GetEmployeeById(int id);
        public List<Employee> GetEmployees();
        public Employee UpdateEmployee(Employee employee);
        public bool DeleteEmployee(int id);

    }
}
