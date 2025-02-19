using System.Collections.ObjectModel;
using System.Windows.Input;
using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using ProjectManagementApp.UI.Helpers;
using ProjectManagementApp.UI.Interfaces;
using ProjectManagement.DAL.Models;

namespace ProjectManagementApp.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IProject _projectService;
        private readonly IEmployee _employeeService;
        private readonly IWindowService _windowService;

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public ICommand AddProjectCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }
        public ICommand EditProjectCommand { get; set; }
        public ICommand DeleteProjectCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        public ICommand ApplyFilterCommand { get; set; }
        public ICommand ResetSearchCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public string SelectedPriority { get; set; }
        public string SearchQuery { get; set; }

        public List<string> PriorityOptions { get; set; }

        public MainViewModel(IProject projectService, IEmployee employeeService, IWindowService windowService)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));

            LoadData();

            PriorityOptions = new List<string> { "Low", "Medium", "High" };

            AddProjectCommand = new RelayCommand(OpenAddProjectWizard);
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeDialog);
            EditEmployeeCommand = new RelayCommand<Employee>(EditEmployee);
            DeleteEmployeeCommand = new RelayCommand<Employee>(DeleteEmployee);
            EditProjectCommand = new RelayCommand<Project>(EditProject);
            DeleteProjectCommand = new RelayCommand<Project>(DeleteProject);
            ApplyFilterCommand = new RelayCommand(FilterAndSortProjects);
            ResetSearchCommand = new RelayCommand(ResetFilters);
            SearchCommand = new RelayCommand(FilterAndSortProjects);
        }

        private void LoadData()
        {
            Projects = new ObservableCollection<Project>(_projectService.GetAllProjects());
            Employees = new ObservableCollection<Employee>(_employeeService.GetAllEmployees());
        }

        private void OpenAddProjectWizard()
        {
            _windowService.ShowAddProjectWindow(OnProjectAdded);
        }

        private void OpenAddEmployeeDialog()
        {
            _windowService.ShowAddEmployeeWindow(OnEmployeeAdded);
        }

        private void EditProject(Project project)
        {
            if (project != null)
            {
                _windowService.ShowEditProjectWindow(project, OnProjectUpdated);
            }
        }

        private void EditEmployee(Employee employee)
        {
            if (employee != null)
            {
                _windowService.ShowEditEmployeeWindow(employee, OnEmployeeUpdated);
            }
        }
        private void DeleteEmployee(Employee employee)
        {
            if (employee != null)
            {
                _employeeService.DeleteEmployee(employee.Id);
                Employees.Remove(employee);
            }
        }

        private void DeleteProject(Project project)
        {
            if (project != null)
            {
                _projectService.DeleteProject(project.Id);
                Projects.Remove(project);
            }
        }

        private void OnProjectUpdated(Project updatedProject)
        {
            if (updatedProject == null) return;

            var existingProject = Projects.FirstOrDefault(p => p.Id == updatedProject.Id);
            if (existingProject != null)
            {
                int index = Projects.IndexOf(existingProject);
                Projects[index] = updatedProject;
            }
        }

        private void OnEmployeeUpdated(Employee updatedEmployee)
        {
            if (updatedEmployee == null) return;

            var existingEmployee = Employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);
            if (existingEmployee != null)
            {
                existingEmployee.FirstName = updatedEmployee.FirstName;
                existingEmployee.LastName = updatedEmployee.LastName;
                existingEmployee.Email = updatedEmployee.Email;

                var index = Employees.IndexOf(existingEmployee);
                Employees.RemoveAt(index);
                Employees.Insert(index, existingEmployee);
            }
        }

        private void OnProjectAdded(Project newProject)
        {
            if (newProject == null) return;
            Projects.Add(newProject);
        }

        private void OnEmployeeAdded(Employee newEmployee)
        {
            if (newEmployee == null) return;
            Employees.Add(newEmployee);
        }

        private void FilterAndSortProjects()
        {
            var filterOptions = new ProjectFilterOptions
            {
                StartDate = FilterStartDate,
                EndDate = FilterEndDate,
                Priority = SelectedPriority switch
                {
                    "Low" => ProjectManagement.DAL.Enum.Priority.Low,
                    "Medium" => ProjectManagement.DAL.Enum.Priority.Medium,
                    "High" => ProjectManagement.DAL.Enum.Priority.High,
                    _ => null
                },
                ProjectName = SearchQuery
            };

            var filteredProjects = _projectService.FilterAndSortProjects(filterOptions);
            Projects.Clear();
            foreach (var project in filteredProjects)
            {
                Projects.Add(project);
            }
        }

        private void ResetFilters()
        {
            FilterStartDate = null;
            FilterEndDate = null;
            SelectedPriority = null;
            SearchQuery = null;

            Projects.Clear();
            foreach (var project in _projectService.GetAllProjects())
            {
                Projects.Add(project);
            }
        }
    }
}