using EmployeeJwtAuthentication.Models;
using EmployeeJwtAuthentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace EmployeeJwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;       
        }

        [HttpGet]
        [Route("getemployees")]
        public IResult GetEmployees()
        {
            var employees = _employeeService.GetEmployees();
            return Results.Ok(employees);
        }

        [HttpGet]
        [Route("getemployeebyid")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Member, Administrator")]
        public IResult GetEmployeeById(int employeeId)
        {
            var employee = _employeeService.GetEmployeeById(employeeId);
            if (employee == null)
            {
                return Results.NotFound("Employee does not exist");
            }
            return Results.Ok(employee);
        }

        [HttpPost]
        [Route("addemployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrator, Member")]
        public IResult AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            var result = _employeeService.AddEmployee(employee);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("updateemployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrator")]
        public IResult UpdateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            var updatedEmployee = _employeeService.UpdateEmployee(employee);
            if (updatedEmployee == null)
            {
                return Results.NotFound("Employee does not exist");
            }
            return Results.Ok(updatedEmployee);
        }

        [HttpDelete]
        [Route("deleteemployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrator")]
        public IResult DeleteEmployee(int employeeId)
        {
            var result = _employeeService.DeleteEmployee(employeeId);
            if (!result)
            {
                return Results.BadRequest("Failed to delete employee");
            }
            return Results.Ok(result);
        }
    }
}
