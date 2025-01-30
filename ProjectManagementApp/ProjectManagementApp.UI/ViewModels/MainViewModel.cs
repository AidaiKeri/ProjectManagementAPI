using System.Collections.ObjectModel;
using System.Windows.Input;
using ProjectManagementAPI.Entities;
using ProjectManagementAPI.Interfaces;
using ProjectManagementApp.UI.Helpers;
using ProjectManagementApp.UI.Interfaces;

namespace ProjectManagementApp.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IProject _projectService;
        private readonly IEmployee _employeeService;
        private readonly IWindowService _windowService;

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        private List<Project> _allProjects;

        public ICommand AddProjectCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }
        public ICommand EditProjectCommand { get; set; }
        public ICommand DeleteProjectCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        public ICommand ApplyFilterCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ResetSearchCommand { get; set; }

        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public string SelectedPriority { get; set; }

        public List<string> PriorityOptions { get; set; }

        public MainViewModel(IProject projectService, IEmployee employeeService, IWindowService windowService)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));

            _allProjects = _projectService.GetAllProjects();
            Projects = new ObservableCollection<Project>(_allProjects);
            Employees = new ObservableCollection<Employee>(_employeeService.GetAllEmployees());

            PriorityOptions = new List<string> { "Low", "Medium", "High" };

            AddProjectCommand = new RelayCommand(OpenAddProjectWizard);
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeDialog);
            EditEmployeeCommand = new RelayCommand<Employee>(EditEmployee);
            DeleteEmployeeCommand = new RelayCommand<Employee>(DeleteEmployee);
            EditProjectCommand = new RelayCommand<Project>(EditProject);
            DeleteProjectCommand = new RelayCommand<Project>(DeleteProject);
            ApplyFilterCommand = new RelayCommand(FilterAndSortProjects);
            SearchCommand = new RelayCommand(FilterAndSortProjects);
            ResetSearchCommand = new RelayCommand(ResetFilters);
        }

        private void OpenAddProjectWizard()
        {
            _windowService.ShowAddProjectWindow(OnProjectAdded);
        }

        private void OpenAddEmployeeDialog()
        {
            _windowService.ShowAddEmployeeWindow(OnEmployeeAdded);
        }

        private void EditProject(object parameter)
        {
            var project = parameter as Project;
            if (project != null)
            {
                _windowService.ShowEditProjectWindow(project, OnProjectUpdated);
            }
        }

        private void EditEmployee(object parameter)
        {
            var employee = parameter as Employee;
            if (employee != null)
            {
                _windowService.ShowEditEmployeeWindow(employee, OnEmployeesUpdated);
            }
        }

        private void DeleteEmployee(object parameter)
        {
            var employee = parameter as Employee;
            if (employee != null)
            {
                Employees.Remove(employee);
                _employeeService.DeleteEmployee(employee.Id);
            }
        }

        private void DeleteProject(object parameter)
        {
            var project = parameter as Project;
            if (project != null)
            {
                Projects.Remove(project);
                _projectService.DeleteProject(project.Id);
            }
        }

        private void OnProjectUpdated(Project updatedProject)
        {
            if (updatedProject == null) return;

            var existingProject = Projects.FirstOrDefault(p => p.Id == updatedProject.Id);
            if (existingProject != null)
            {
                int index = Projects.IndexOf(existingProject);
                Projects.RemoveAt(index); 
                Projects.Insert(index, updatedProject);
            }

            _projectService.UpdateProject(updatedProject);
        }

        private void OnEmployeesUpdated(Employee updatedEmployee)
        {
            if (updatedEmployee == null) return;

            var existingEmployee = Employees.FirstOrDefault(p => p.Id == updatedEmployee.Id);
            if (existingEmployee != null)
            {
                int index = Employees.IndexOf(existingEmployee);
                Employees.RemoveAt(index);
                Employees.Insert(index, existingEmployee);
            }

            _employeeService.UpdateEmployee(updatedEmployee);
        }

        private void OnProjectAdded(Project newProject)
        {
            if (newProject == null) return;

            Projects.Add(newProject);
            OnPropertyChanged(nameof(Projects));
        }

        private void OnEmployeeAdded(Employee newEmployee)
        {
            if (newEmployee == null) return;

            Employees.Add(newEmployee);
            OnPropertyChanged(nameof(Employees));
        }

        private void FilterAndSortProjects()
        {
            IEnumerable<Project> filteredProjects = _allProjects;

            if (FilterStartDate.HasValue && FilterEndDate.HasValue)
            {
                filteredProjects = _projectService.FilterProjectsByDateRange(FilterStartDate.Value, FilterEndDate.Value);
            }

            if (!string.IsNullOrEmpty(SelectedPriority))
            {
                int priorityValue = SelectedPriority switch
                {
                    "Low" => 1,
                    "Medium" => 2,
                    "High" => 3,
                    _ => 0
                };

                if (priorityValue > 0)
                {
                    filteredProjects = _projectService.FilterProjectsByPriority(priorityValue);
                }
            }

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

            Projects.Clear();
            foreach (var project in _allProjects)
            {
                Projects.Add(project);
            }
        }
    }
}

