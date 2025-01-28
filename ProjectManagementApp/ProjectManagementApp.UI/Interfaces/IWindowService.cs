using ProjectManagementAPI.Entities;

namespace ProjectManagementApp.UI.Interfaces
{
    public interface IWindowService
    {
        void ShowAddProjectWindow(Action<Project> onProjectAdded);
    }
}
