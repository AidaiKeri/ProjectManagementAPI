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
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com"
        },
        new Employee
        {
            Id = 2,
            FirstName = "Kate",
            LastName = "Smith",
            Email = "katesmith@example.com"
        }
    };
    }
}
