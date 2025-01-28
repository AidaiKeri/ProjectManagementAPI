using ProjectManagementAPI.Entities;

namespace ProjectManagementAPI.DataAccess
{
    public static class MockData
    {
        public static List<Project> Projects = new List<Project>();
        public static List<Employee> Employees = new List<Employee>
    {
        new Employee
        {
            Id = 1,
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice.johnson@example.com"
        },
        new Employee
        {
            Id = 2,
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@example.com"
        }
    };
    }
}
