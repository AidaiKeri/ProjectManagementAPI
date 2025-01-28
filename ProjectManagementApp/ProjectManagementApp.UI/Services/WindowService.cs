using ProjectManagementAPI.Entities;
using ProjectManagementAPI.Interfaces;
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
            // Получаем список сотрудников
            var allEmployees = _employeeService.GetAllEmployees(); 

            // Создаем окно и передаем все параметры
            var addProjectWindow = new AddProjectWindow(_projectService, onProjectAdded, allEmployees);
            addProjectWindow.ShowDialog();
        }
    }
}
