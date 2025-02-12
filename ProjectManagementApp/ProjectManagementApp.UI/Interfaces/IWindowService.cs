using ProjectManagement.DAL.Entities;

namespace ProjectManagementApp.UI.Interfaces
{
    public interface IWindowService
    {
        void ShowAddProjectWindow(Action<Project> onProjectAdded);
        void ShowEditProjectWindow(Project project, Action<Project> onProjectUpdated);
        void ShowAddEmployeeWindow(Action<Employee> onEmployeeAdded);
        void ShowEditEmployeeWindow(Employee employee, Action<Employee> onEmployeeUpdated);
    }
}
