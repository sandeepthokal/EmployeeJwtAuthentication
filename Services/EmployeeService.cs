using EmployeeJwtAuthentication.Models;
using EmployeeJwtAuthentication.Repositories;

namespace EmployeeJwtAuthentication.Services
{
    public class EmployeeService : IEmployeeService
    {
        public Employee AddEmployee(Employee employee)
        {
            employee.Id = EmployeeRepository.Employees.MaxBy(m => m.Id).Id + 1; 
            EmployeeRepository.Employees.Add(employee);
            return employee;
        }

        public Employee GetEmployeeById(int id)
        {
            var employee = EmployeeRepository.Employees.FirstOrDefault(e => e.Id == id);
            return employee;
        } 
        public List<Employee> GetEmployees()
        {
            return EmployeeRepository.Employees;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var existingEmployee = EmployeeRepository.Employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existingEmployee == null)
            {
                return null;
            }
            else
            {
                employee.Id = existingEmployee.Id;
            }
            return employee;
        }

        public bool DeleteEmployee(int id)
        {
            var existingEmployee = EmployeeRepository.Employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
            {
                return false;
            }
            EmployeeRepository.Employees.Remove(existingEmployee);
            return true;
        }
    }
}
