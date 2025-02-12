using Microsoft.AspNetCore.Mvc;
using ProjectManagement.BLL.Interfaces;
using ProjectManagement.DAL.Entities;
using ProjectManagement.DAL.Models;

namespace ProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeService;

        public EmployeeController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        [HttpGet]
        [Route("get-all-employees")]
        public ActionResult<List<Employee>> GetAllEmployees()
        {
            return Ok(_employeeService.GetAllEmployees());
        }

        /// <summary>
        /// Get employee by id
        /// </summary>
        [HttpGet]
        [Route("get-employee-by-id")]
        public ActionResult<Employee> GetEmployeeById(string id)
        {
            var employee = _employeeService.GetEmployeeById(Guid.Parse(id));
            if (employee == null) return NotFound($"Employee with ID  {id}  not found.");
            return Ok(employee);
        }

        /// <summary>
        /// Create employee
        /// </summary>
        [HttpPost]
        [Route("create-employee")]
        public ActionResult<Employee> CreateEmployee([FromBody] CreateEmployeeModel request)
        {
            var employee = _employeeService.CreateEmployee(request);
            return Ok(employee);
        }

        /// <summary>
        /// Edit employee data
        /// </summary>
        [HttpPut]
        [Route("edit-employee")]
        public ActionResult UpdateEmployee(string id, [FromBody] CreateEmployeeModel employee)
        {
            _employeeService.UpdateEmployee(Guid.Parse(id), employee);
            return NoContent();
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        [HttpDelete]
        [Route("delete-employee")]
        public ActionResult DeleteEmployee(string id)
        {
            var employee = _employeeService.GetEmployeeById(Guid.Parse(id));
            if (employee == null) return NotFound($"Employee with ID {id} not found.");

            _employeeService.DeleteEmployee(Guid.Parse(id));
            return NoContent();
        }
    }
}

