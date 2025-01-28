using ProjectManagementAPI.DataAccess;
using ProjectManagementAPI.Entities;
using ProjectManagementAPI.Interfaces;

namespace ProjectManagementAPI.Services
{
    public class EmployeeService : IEmployee
    {
        public List<Employee> GetAllEmployees() => MockData.Employees;

        public Employee GetEmployeeById(int id) => MockData.Employees.FirstOrDefault(e => e.Id == id);

        public void CreateEmployee(Employee employee)
        {
            employee.Id = MockData.Employees.Any() ? MockData.Employees.Max(e => e.Id) + 1 : 1;
            MockData.Employees.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            var existingEmployee = GetEmployeeById(employee.Id);
            if (existingEmployee != null)
            {
                MockData.Employees.Remove(existingEmployee);
                MockData.Employees.Add(employee);
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee != null)
            {
                MockData.Employees.Remove(employee);
            }
        }
    }
}
