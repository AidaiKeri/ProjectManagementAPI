using ProjectManagement.DAL.Entities;
using ProjectManagement.DAL.Models;

namespace ProjectManagement.BLL.Interfaces
{
    public interface IProject
    {
        List<Project> GetAllProjects();
        Project GetProjectById(Guid id);
        Project CreateProject(CreateProjectModel project);
        void UpdateProject(Guid id, CreateProjectModel project);
        void DeleteProject(Guid id);
        void RemoveEmployeeFromProject(Guid projectId, Guid employeeId);
        void AddEmployeeToProject(Guid projectId, Guid employeeId);
        List<Project> FilterAndSortProjects(ProjectFilterOptions filterOptions);
    }
}
