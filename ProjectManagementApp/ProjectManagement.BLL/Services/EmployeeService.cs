using ProjectManagement.DAL.DataAccess;
using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using ProjectManagement.DAL.Models;

namespace ProjectManagement.BLL.Services
{
    public class EmployeeService : IEmployee
    {
        private readonly WebApiDbContext _context;

        public EmployeeService(WebApiDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployeeById(Guid id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == id);
        }

        public Employee CreateEmployee(CreateEmployeeModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Employee cannot be null");
            }

            if (_context.Employees.Any(e => e.Email == request.Email))
            {
                throw new InvalidOperationException($"Employee with email {request.Email} already exists.");
            }

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return employee; 
        }

        public void UpdateEmployee(Guid id, CreateEmployeeModel employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Employee cannot be null");
            }

            var existingEmployee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;

            _context.SaveChanges();
        }

        public void DeleteEmployee(Guid id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
