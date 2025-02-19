using ProjectManagement.DAL.Entities;
using ProjectManagement.BLL.Interfaces;
using System.Windows;
using ProjectManagement.DAL.Models;

namespace ProjectManagementApp.UI.Views
{
    /// <summary>
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private readonly IEmployee _employeeService;
        private readonly Action<Employee> _onEmployeeAdded;

        public AddEmployeeWindow(IEmployee employeeService, Action<Employee> onEmployeeAdded)
        {
            InitializeComponent();
            _employeeService = employeeService;
            _onEmployeeAdded = onEmployeeAdded;
        }

        private void OnAddEmployeeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var employeeFirstName = EmployeeFirstName.Text;
                var employeeLastName = EmployeeLastName.Text;
                var employeeEmail = EmployeeEmail.Text;

                if (string.IsNullOrWhiteSpace(employeeFirstName) ||
                    string.IsNullOrWhiteSpace(employeeLastName) ||
                    string.IsNullOrWhiteSpace(employeeEmail))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newEmployeeModel = new CreateEmployeeModel
                {
                    FirstName = employeeFirstName,
                    LastName = employeeLastName,
                    Email = employeeEmail
                };

                var createdEmployee = _employeeService.CreateEmployee(newEmployeeModel);

                var newEmployee = new Employee
                {
                    Id = createdEmployee.Id, 
                    FirstName = createdEmployee.FirstName,
                    LastName = createdEmployee.LastName,
                    Email = createdEmployee.Email
                };

                _onEmployeeAdded(newEmployee);

                MessageBox.Show("Employee added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
