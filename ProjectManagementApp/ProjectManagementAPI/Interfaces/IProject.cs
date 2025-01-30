using ProjectManagementAPI.Entities;

namespace ProjectManagementAPI.Interfaces
{
    public interface IProject
    {
        List<Project> GetAllProjects();
        Project GetProjectById(int id);
        void CreateProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(int id);
        List<Project> FilterProjectsByDateRange(DateTime startDate, DateTime endDate);
        List<Project> FilterProjectsByPriority(int priority);
    }
}
