using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.BLL.Interfaces;
using ProjectManagement.BLL.Services;
using ProjectManagementApp.UI.Interfaces;
using ProjectManagementApp.UI.ViewModels;
using ProjectManagementApp.UI.Services;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL.DataAccess;

namespace ProjectManagementApp.UI
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<WebApiDbContext>(options =>
                options.UseNpgsql("Server=localhost;Database=ProjectApi;Port=5430;User Id=postgres;Password=1123581321;Trust Server Certificate=true;"));

            serviceCollection.AddScoped<IProject, ProjectService>();
            serviceCollection.AddScoped<IEmployee, EmployeeService>();
            serviceCollection.AddSingleton<IWindowService, WindowService>();

            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<MainWindow>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}



