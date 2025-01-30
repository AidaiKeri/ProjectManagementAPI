using ProjectManagementAPI.Entities;
using ProjectManagementAPI.Interfaces;
using System.Windows;

namespace ProjectManagementApp.UI.Views
{
    /// <summary>
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        private readonly IEmployee _employeeService;
        private readonly Employee _employee;
        private readonly Action<Employee> _onEmployeeUpdated;

        public EditEmployeeWindow(IEmployee employeeService, Employee employee, Action<Employee> onEmployeeUpdated)
        {
            InitializeComponent();
            _employeeService = employeeService;
            _employee = employee;
            _onEmployeeUpdated = onEmployeeUpdated;

            EmployeeFirstName.Text = _employee.FirstName;
            EmployeeLastName.Text = _employee.LastName;
            EmployeeEmail.Text = _employee.Email;
        }
        private void OnSaveChangesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _employee.FirstName = EmployeeFirstName.Text;
                _employee.LastName = EmployeeLastName.Text;
                _employee.Email = EmployeeEmail.Text;

                _employeeService.UpdateEmployee(_employee);
                _onEmployeeUpdated?.Invoke(_employee);

                MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
