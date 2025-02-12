using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using ProjectManagementApp.UI.Interfaces;
using ProjectManagementApp.UI.Views;

namespace ProjectManagementApp.UI.Services
{
    public class WindowService : IWindowService
    {
        private readonly IProject _projectService;
        private readonly IEmployee _employeeService; 

        public WindowService(IProject projectService, IEmployee employeeService)
        {
            _projectService = projectService;
            _employeeService = employeeService;
        }

        public void ShowAddProjectWindow(Action<Project> onProjectAdded)
        {
            var allEmployees = _employeeService.GetAllEmployees(); 

            var addProjectWindow = new AddProjectWindow(_projectService, onProjectAdded, allEmployees);
            addProjectWindow.ShowDialog();
        }

        public void ShowEditProjectWindow(Project project, Action<Project> onProjectUpdated)
        {
            var editProjectWindow = new EditProjectWindow(_projectService, _employeeService, project, onProjectUpdated);
            editProjectWindow.ShowDialog();
        }

        public void ShowAddEmployeeWindow(Action<Employee> onEmployeeAdded)
        {
            var addEmployeeWindow = new AddEmployeeWindow(_employeeService, onEmployeeAdded);
            addEmployeeWindow.ShowDialog();
        }

        public void ShowEditEmployeeWindow(Employee employee, Action<Employee> onEmployeeUpdated)
        {
            var editEmployeeWindow = new EditEmployeeWindow(_employeeService, employee, onEmployeeUpdated);
            editEmployeeWindow.ShowDialog();
        }
    }
}
