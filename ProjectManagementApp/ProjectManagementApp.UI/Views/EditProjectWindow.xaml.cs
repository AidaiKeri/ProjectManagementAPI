using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using System.Windows;
using System.Windows.Controls;
using ProjectManagement.DAL.Models;

namespace ProjectManagementApp.UI.Views
{
    /// <summary>
    /// Interaction logic for EditProjectWindow.xaml
    /// </summary>
    public partial class EditProjectWindow : Window
    {
        private readonly IProject _projectService;
        private readonly IEmployee _employeeService;
        private readonly Project _project;
        private readonly Action<Project> _onProjectUpdated;
        private readonly List<Employee> _allEmployees;

        public EditProjectWindow(IProject projectService, IEmployee employeeService, Project project, Action<Project> onProjectUpdated)
        {
            InitializeComponent();
            _projectService = projectService;
            _employeeService = employeeService;
            _project = project;
            _onProjectUpdated = onProjectUpdated;
            _allEmployees = _employeeService.GetAllEmployees();

            ProjectName.Text = _project.Name;
            CustomerCompany.Text = _project.CustomerCompany;
            ContractorCompany.Text = _project.ContractorCompany;
            StartDate.SelectedDate = _project.StartDate;
            EndDate.SelectedDate = _project.EndDate;
            PriorityComboBox.SelectedItem = PriorityComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _project.Priority.ToString());
            ProjectManagerComboBox.ItemsSource = _allEmployees;
            ProjectManagerComboBox.SelectedItem = _project.ProjectManager;

            ProjectEmployeesListBox.ItemsSource = _project.Employees;
            AllEmployeesListBox.ItemsSource = _allEmployees.Except(_project.Employees).ToList();
        }

        private void OnAddEmployeeToProjectClick(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = AllEmployeesListBox.SelectedItem as Employee;
            if (selectedEmployee != null && !_project.Employees.Contains(selectedEmployee))
            {
                _project.Employees.Add(selectedEmployee);

                var createProjectModel = new CreateProjectModel
                {
                    Name = _project.Name,
                    CustomerCompany = _project.CustomerCompany,
                    ContractorCompany = _project.ContractorCompany,
                    StartDate = _project.StartDate,
                    EndDate = _project.EndDate,
                    Priority = _project.Priority,
                    ProjectManagerId = _project.ProjectManager?.Id,
                };

                _projectService.UpdateProject(_project.Id, createProjectModel);

                RefreshEmployeeLists();
            }
        }

        private void OnRemoveEmployeeFromProjectClick(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = ProjectEmployeesListBox.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                _project.Employees.Remove(selectedEmployee);

                var createProjectModel = new CreateProjectModel
                {
                    Name = _project.Name,
                    CustomerCompany = _project.CustomerCompany,
                    ContractorCompany = _project.ContractorCompany,
                    StartDate = _project.StartDate,
                    EndDate = _project.EndDate,
                    Priority = _project.Priority,
                    ProjectManagerId = _project.ProjectManager?.Id,
                };

                _projectService.UpdateProject(_project.Id, createProjectModel);

                RefreshEmployeeLists();
            }
        }

        private void RefreshEmployeeLists()
        {
            ProjectEmployeesListBox.ItemsSource = null;
            ProjectEmployeesListBox.ItemsSource = _project.Employees;

            AllEmployeesListBox.ItemsSource = null;
            AllEmployeesListBox.ItemsSource = _allEmployees.Except(_project.Employees).ToList();
        }

        private void OnSaveChangesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var startDateUtc = StartDate.SelectedDate.Value.ToUniversalTime();
                var endDateUtc = EndDate.SelectedDate.Value.ToUniversalTime();

                var createProjectModel = new CreateProjectModel
                {
                    Name = ProjectName.Text,
                    CustomerCompany = CustomerCompany.Text,
                    ContractorCompany = ContractorCompany.Text,
                    StartDate = startDateUtc,
                    EndDate = endDateUtc,
                    Priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() switch
                    {
                        "Low" => ProjectManagement.DAL.Enum.Priority.Low,
                        "Medium" => ProjectManagement.DAL.Enum.Priority.Medium,
                        "High" => ProjectManagement.DAL.Enum.Priority.High,
                    },
                    ProjectManagerId = (ProjectManagerComboBox.SelectedItem as Employee)?.Id,
                };

                _projectService.UpdateProject(_project.Id, createProjectModel);

                _onProjectUpdated?.Invoke(_project);

                MessageBox.Show("Project updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
