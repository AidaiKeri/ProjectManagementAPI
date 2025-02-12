using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using System.Windows;
using System.Windows.Controls;

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

                int priority = priorityString switch
                {
                    "Low" => 1,
                    "Medium" => 2,
                    "High" => 3,
                    _ => 0
                };

                var newProject = new Project
                {
                    Name = projectName,
                    CustomerCompany = customerCompany,
                    ContractorCompany = contractorCompany,
                    StartDate = startDate.Value,
                    EndDate = endDate.Value,
                    //Priority = priority,
                    ProjectManager = projectManager, 
                    Employees = selectedEmployees 
                };

                //_projectService.CreateProject(newProject);
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
