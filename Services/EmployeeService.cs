using EmployeeJwtAuthentication.Models;
using EmployeeJwtAuthentication.Repositories;

namespace EmployeeJwtAuthentication.Services
{
    /// <summary>
    /// Employee services handles the CRUD operations on Employee entity
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// Method Used to create employee 
        /// </summary>
        /// <param name="employee">Contains parameters required for Employee Entity</param>
        /// <returns>Employee instance</returns>
        public Employee AddEmployee(Employee employee)
        {
            employee.Id = EmployeeRepository.Employees.MaxBy(m => m.Id).Id + 1; 
            EmployeeRepository.Employees.Add(employee);
            return employee;
        }

        /// <summary>
        /// Method used to get employee based on employee if provided in the input
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>Employee details</returns>
        public Employee GetEmployeeById(int id)
        {
            var employee = EmployeeRepository.Employees.FirstOrDefault(e => e.Id == id);
            return employee;
        }
        
        /// <summary>
        /// List's all the employees
        /// </summary>
        /// <returns>List of employees</returns>
        public List<Employee> GetEmployees()
        {
            return EmployeeRepository.Employees;
        }

        /// <summary>
        /// Method used to Update existing employee details
        /// </summary>
        /// <param name="employee">New employee instance</param>
        /// <returns>Updated employee details</returns>
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

        /// <summary>
        /// Method Used to delete employee details
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>true or false</returns>
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
