using ProjectManagement.DAL.Entities;
using ProjectManagement.DAL.Models;

namespace ProjectManagement.BLL.Interfaces
{
    public interface IEmployee
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(Guid id);
        Employee CreateEmployee(CreateEmployeeModel request);
        void UpdateEmployee(Guid id, CreateEmployeeModel employee);
        void DeleteEmployee(Guid id);
    }
}
