using ProjectManagementAPI.DataAccess;
using ProjectManagementAPI.Entities;
using ProjectManagementAPI.Interfaces;

namespace ProjectManagementAPI.Services
{
    public class ProjectService : IProject
    {
        public List<Project> GetAllProjects() => MockData.Projects;

        public Project GetProjectById(int id) => MockData.Projects.FirstOrDefault(p => p.Id == id);

        public void CreateProject(Project project)
        {
            project.Id = MockData.Projects.Any() ? MockData.Projects.Max(p => p.Id) + 1 : 1;
            MockData.Projects.Add(project);
        }

        public void UpdateProject(Project project)
        {
            var existingProject = GetProjectById(project.Id);
            if (existingProject != null)
            {
                MockData.Projects.Remove(existingProject);
                MockData.Projects.Add(project);
            }
        }

        public void DeleteProject(int id)
        {
            var project = GetProjectById(id);
            if (project != null)
            {
                MockData.Projects.Remove(project);
            }
        }

        public List<Project> FilterProjectsByDateRange(DateTime startDate, DateTime endDate)
        {
            return MockData.Projects
                .Where(p => p.StartDate >= startDate && p.EndDate <= endDate)
                .ToList();
        }

        public List<Project> FilterProjectsByPriority(int priority)
        {
            return MockData.Projects
                .Where(p => p.Priority == priority)
                .ToList();
        }
    }
}
