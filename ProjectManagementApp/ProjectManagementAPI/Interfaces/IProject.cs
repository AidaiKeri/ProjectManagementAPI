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
        void AddEmployeeToProject(int projectId, int employeeId);
        void RemoveEmployeeFromProject(int projectId, int employeeId);
        List<Project> FilterProjectsByDateRange(DateTime startDate, DateTime endDate);
        List<Project> FilterProjectsByPriority(int priority);
        List<Project> SortProjectsByField(Func<Project, object> keySelector, bool ascending = true);
    }
}
