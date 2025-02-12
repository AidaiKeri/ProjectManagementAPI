using Microsoft.AspNetCore.Mvc;
using ProjectManagement.BLL.Interfaces;
using ProjectManagement.DAL.Entities;
using ProjectManagement.DAL.Models;
using ProjectManagement.DAL.Enum;

namespace ProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProject _projectService;

        public ProjectsController(IProject projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        [HttpGet]
        [Route("get-all-projects")]
        public ActionResult<List<Project>> GetAllProjects()
        {
            return Ok(_projectService.GetAllProjects());
        }

        /// <summary>
        /// Get project by id
        /// </summary>
        [HttpGet]
        [Route("get-project-by-id")]
        public ActionResult<Project> GetProjectById(string id)
        {
            var project = _projectService.GetProjectById(Guid.Parse(id));
            if (project == null) return NotFound($"Project with ID {id} not found.");
            return Ok(project);
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        [HttpPost]
        [Route("create-project")]
        public ActionResult CreateProject([FromBody] CreateProjectModel project)
        {
            var employee = _projectService.CreateProject(project);
            return Ok(employee);
        }

        /// <summary>
        /// Update an existing project
        /// </summary>
        [HttpPut]
        [Route("edit-project")]
        public ActionResult UpdateProject(string id, [FromBody] CreateProjectModel project)
        {
            _projectService.UpdateProject(Guid.Parse(id), project);
            return NoContent();
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        [HttpDelete]
        [Route("delete-project")]
        public ActionResult DeleteProject(string id)
        {
            var project = _projectService.GetProjectById(Guid.Parse(id));
            if (project == null) return NotFound($"Project with ID {id} not found.");

            _projectService.DeleteProject(Guid.Parse(id));
            return NoContent();
        }

        /// <summary>
        /// Add an employee to a project
        /// </summary>
        [HttpPost]
        [Route("add-employee-to-project")]
        public ActionResult AddEmployeeToProject(string projectId, string employeeId)
        {
            try
            {
                _projectService.AddEmployeeToProject(Guid.Parse(projectId), Guid.Parse(employeeId));
                return Ok($"Employee {employeeId} added to project {projectId}.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Remove an employee from a project
        /// </summary>
        [HttpDelete]
        [Route("delete-employee-from-project")]
        public ActionResult RemoveEmployeeFromProject(string projectId, string employeeId)
        {
            try
            {
                _projectService.RemoveEmployeeFromProject(Guid.Parse(projectId), Guid.Parse(employeeId));
                return Ok($"Employee {employeeId} removed from project {projectId}.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Filter and sort projects
        /// </summary>
        [HttpGet]
        [Route("get-filtered-project")]
        public ActionResult<List<Project>> FilterProjects(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] Priority priority,
            [FromQuery] string projectName,
            [FromQuery] string sortBy = "name",
            [FromQuery] bool sortDescending = false)
        {
            var filterOptions = new ProjectFilterOptions
            {
                StartDate = startDate,
                EndDate = endDate,
                Priority = priority,
                ProjectName = projectName,
                SortBy = sortBy,
                SortDescending = sortDescending
            };

            var projects = _projectService.FilterAndSortProjects(filterOptions);
            return Ok(projects);
        }
    }
}



