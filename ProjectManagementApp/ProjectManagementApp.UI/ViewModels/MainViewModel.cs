using System.Collections.ObjectModel;
using System.Windows.Input;
using ProjectManagementAPI.Entities;
using ProjectManagementAPI.Interfaces;
using ProjectManagementApp.UI.Helpers;
using ProjectManagementApp.UI.Interfaces;

public class MainViewModel : BaseViewModel
{
    private readonly IProject _projectService;
    private readonly IEmployee _employeeService;
    private readonly IWindowService _windowService;

    public ObservableCollection<Project> Projects { get; set; }
    public ObservableCollection<Employee> Employees { get; set; }

    public ICommand AddProjectCommand { get; set; }
    public ICommand AddEmployeeCommand { get; set; }

    public MainViewModel(IProject projectService, IEmployee employeeService, IWindowService windowService)
    {
        _projectService = projectService;
        _employeeService = employeeService;
        _windowService = windowService;

        Projects = new ObservableCollection<Project>(_projectService.GetAllProjects());
        Employees = new ObservableCollection<Employee>(_employeeService.GetAllEmployees());

        AddProjectCommand = new RelayCommand(OpenAddProjectWizard);
        AddEmployeeCommand = new RelayCommand(OpenAddEmployeeDialog);
    }

    private void OpenAddProjectWizard()
    {
        // Открываем окно добавления проекта и передаем метод для обновления коллекции
        _windowService.ShowAddProjectWindow(OnProjectAdded);
    }

    private void OpenAddEmployeeDialog()
    {
        // Логика для открытия диалога добавления сотрудника
    }

    // Метод для обновления коллекции после добавления нового проекта
    private void OnProjectAdded(Project newProject)
    {
        Projects.Add(newProject); // Добавляем новый проект в коллекцию
    }
}

