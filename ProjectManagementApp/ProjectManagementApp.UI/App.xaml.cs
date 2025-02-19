using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.BLL.Interfaces;
using ProjectManagement.BLL.Services;
using ProjectManagementApp.UI.Interfaces;
using ProjectManagementApp.UI.ViewModels;
using ProjectManagementApp.UI.Services;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL.DataAccess;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectManagementApp.UI
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
                .Build();

            var connectionString = configuration.GetConnectionString("UiConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'UiConnection' is not found in the configuration.");
            }

            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.AddDbContext<WebApiDbContext>(options =>
                 options.UseNpgsql(connectionString));

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

            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WebApiDbContext>();
                dbContext.Database.Migrate(); 
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}



