using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using System.Windows;
using System.Windows.Controls;
using ProjectManagement.DAL.Models;

namespace ProjectManagementApp.UI.Views
{
    /// <summary>
    /// Interaction logic for AddProjectWindow.xaml
    /// </summary>
    public partial class AddProjectWindow : Window
    {
        private readonly IProject _projectService;
        private readonly Action<Project> _onProjectAdded;
        private readonly List<Employee> _allEmployees; 

        public AddProjectWindow(IProject projectService, Action<Project> onProjectAdded, List<Employee> allEmployees)
        {
            InitializeComponent();
            _projectService = projectService;
            _onProjectAdded = onProjectAdded;
            _allEmployees = allEmployees;

            ProjectManagerComboBox.ItemsSource = _allEmployees;
            EmployeesListBox.ItemsSource = _allEmployees;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            ProjectManagerComboBox.ItemsSource = _allEmployees;
            EmployeesListBox.ItemsSource = _allEmployees;
        }

        private void OnEmployeeFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = EmployeeFilterTextBox.Text.ToLower();
            var filteredEmployees = _allEmployees.Where(emp =>
              emp.FirstName.ToLower().Contains(filter) || emp.LastName.ToLower().Contains(filter))
              .ToList();

            EmployeesListBox.ItemsSource = filteredEmployees;
        }

        private void OnAddProjectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var projectName = ProjectName.Text;
                var customerCompany = CustomerCompany.Text;
                var contractorCompany = ContractorCompany.Text;
                var startDate = StartDate.SelectedDate;
                var endDate = EndDate.SelectedDate;
                var priorityString = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                var projectManager = ProjectManagerComboBox.SelectedItem as Employee;
                var selectedEmployees = EmployeesListBox.SelectedItems.Cast<Employee>().ToList();

                if (string.IsNullOrWhiteSpace(projectName) ||
                    string.IsNullOrWhiteSpace(customerCompany) ||
                    string.IsNullOrWhiteSpace(contractorCompany) ||
                    !startDate.HasValue ||
                    !endDate.HasValue ||
                    string.IsNullOrEmpty(priorityString) ||
                    projectManager == null)
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime startDateUtc = startDate.Value.ToUniversalTime();
                DateTime endDateUtc = endDate.Value.ToUniversalTime();

                var priority = priorityString switch
                {
                    "Low" => ProjectManagement.DAL.Enum.Priority.Low,
                    "Medium" => ProjectManagement.DAL.Enum.Priority.Medium,
                    "High" => ProjectManagement.DAL.Enum.Priority.High,
                    _ => throw new InvalidOperationException("Invalid priority selection")
                };

                var newProjectModel = new CreateProjectModel
                {
                    Name = projectName,
                    CustomerCompany = customerCompany,
                    ContractorCompany = contractorCompany,
                    StartDate = startDateUtc,
                    EndDate = endDateUtc,
                    Priority = priority,
                    ProjectManagerId = projectManager.Id
                };

                var createdProject = _projectService.CreateProject(newProjectModel);

                foreach (var employee in selectedEmployees)
                {
                    _projectService.AddEmployeeToProject(createdProject.Id, employee.Id);
                }

                var newProject = new Project
                {
                    Id = createdProject.Id,
                    Name = createdProject.Name,
                    CustomerCompany = createdProject.CustomerCompany,
                    ContractorCompany = createdProject.ContractorCompany,
                    StartDate = createdProject.StartDate,
                    EndDate = createdProject.EndDate,
                    Priority = createdProject.Priority,
                    ProjectManager = projectManager,
                    Employees = selectedEmployees
                };

                _onProjectAdded(newProject);

                MessageBox.Show("Project added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
