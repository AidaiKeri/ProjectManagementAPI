using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using ProjectManagement.BLL.Services;
using System.Windows;
using System.Windows.Controls;

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
                //_projectService.UpdateProject(_project);
                RefreshEmployeeLists();
            }
        }

        private void OnRemoveEmployeeFromProjectClick(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = ProjectEmployeesListBox.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                _project.Employees.Remove(selectedEmployee);
                //_projectService.UpdateProject(_project);
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
                _project.Name = ProjectName.Text;
                _project.CustomerCompany = CustomerCompany.Text;
                _project.ContractorCompany = ContractorCompany.Text;
                _project.StartDate = StartDate.SelectedDate.Value;
                _project.EndDate = EndDate.SelectedDate.Value;
                //_project.Priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() switch
                //{
                //    "Low" => 1,
                //    "Medium" => 2,
                //    "High" => 3,
                //    _ => 0 
                //};
                _project.ProjectManager = ProjectManagerComboBox.SelectedItem as Employee;

                //_projectService.UpdateProject(_project);
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
