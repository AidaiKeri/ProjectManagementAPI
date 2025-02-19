using ProjectManagement.DAL.DataAccess;
using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL.Models;

namespace ProjectManagement.BLL.Services
{
    public class ProjectService : IProject
    {
        private readonly WebApiDbContext _context;

        public ProjectService(WebApiDbContext context)
        {
            _context = context;
        }

        public List<Project> GetAllProjects()
        {
            return _context.Projects
                           .Include(p => p.ProjectManager) 
                           .Include(p => p.Employees) 
                           .ToList();
        }

        public Project GetProjectById(Guid id)
        {
            return _context.Projects
                           .Include(p => p.ProjectManager)
                           .Include(p => p.Employees)
                           .FirstOrDefault(p => p.Id == id);
        }

        public Project CreateProject(CreateProjectModel project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null");
            }

            if (_context.Projects.Any(p => p.Name == project.Name))
            {
                throw new InvalidOperationException($"Project with name {project.Name} already exists.");
            }

            var projectManager = _context.Employees.FirstOrDefault(p => p.Id == project.ProjectManagerId);

            if (projectManager == null)
            {
                throw new InvalidOperationException($"Project Manager with ID {project.ProjectManagerId} not found.");
            }

            var newProject = new Project
            {
                Id = Guid.NewGuid(),
                Name = project.Name,
                CustomerCompany = project.CustomerCompany,
                ContractorCompany = project.ContractorCompany,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Priority = project.Priority,
                ProjectManagerId = projectManager.Id,
                ProjectManager = projectManager,  
                Employees = new List<Employee>()
            };
            _context.Projects.Add(newProject);
            _context.SaveChanges();

            return newProject;
        }

        public void UpdateProject(Guid id, CreateProjectModel project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null");
            }

            var existingProject = _context.Projects
                                           .Include(p => p.ProjectManager)
                                           .Include(p => p.Employees)
                                           .FirstOrDefault(p => p.Id == id);
            if (existingProject == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} not found.");
            }

            existingProject.Name = project.Name;
            existingProject.CustomerCompany = project.CustomerCompany;
            existingProject.ContractorCompany = project.ContractorCompany;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.Priority = project.Priority;
            existingProject.ProjectManagerId = project.ProjectManagerId;

            _context.SaveChanges();
        }

        public void DeleteProject(Guid id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} not found.");
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();
        }

        public List<Project> FilterAndSortProjects(ProjectFilterOptions filterOptions)
        {
            IQueryable<Project> query = _context.Projects
                                                .Include(p => p.ProjectManager) 
                                                .Include(p => p.Employees); 

            if (filterOptions.StartDate.HasValue)
            {
                query = query.Where(p => p.StartDate >= filterOptions.StartDate.Value);
            }
            if (filterOptions.EndDate.HasValue)
            {
                query = query.Where(p => p.EndDate <= filterOptions.EndDate.Value);
            }

            if (filterOptions.Priority.HasValue)
            {
                query = query.Where(p => p.Priority == filterOptions.Priority.Value);
            }

            if (!string.IsNullOrEmpty(filterOptions.ProjectName))
            {
                query = query.Where(p => p.Name.Contains(filterOptions.ProjectName));
            }

            if (!string.IsNullOrEmpty(filterOptions.SortBy))
            {
                switch (filterOptions.SortBy.ToLower())
                {
                    case "name":
                        query = filterOptions.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                        break;
                    case "startdate":
                        query = filterOptions.SortDescending ? query.OrderByDescending(p => p.StartDate) : query.OrderBy(p => p.StartDate);
                        break;
                    case "enddate":
                        query = filterOptions.SortDescending ? query.OrderByDescending(p => p.EndDate) : query.OrderBy(p => p.EndDate);
                        break;
                    case "priority":
                        query = filterOptions.SortDescending ? query.OrderByDescending(p => p.Priority) : query.OrderBy(p => p.Priority);
                        break;
                    default:
                        query = query.OrderBy(p => p.Name); 
                        break;
                }
            }

            return query.ToList();
        }

        public void AddEmployeeToProject(Guid projectId, Guid employeeId)
        {
            var project = _context.Projects.Include(p => p.Employees).FirstOrDefault(p => p.Id == projectId);

            var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");
            }

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            }

            if (!project.Employees.Contains(employee))
            {
                project.Employees.Add(employee);               
            }

            _context.SaveChanges();
        }

        public void RemoveEmployeeFromProject(Guid projectId, Guid employeeId)
        {
            var project = _context.Projects.Include(p => p.Employees).FirstOrDefault(p => p.Id == projectId);

            var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");
            }

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            }

            if (project.Employees.Contains(employee))
            {
                project.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }
    }
}

