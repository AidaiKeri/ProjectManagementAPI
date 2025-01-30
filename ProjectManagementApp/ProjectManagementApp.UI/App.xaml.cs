using Microsoft.Extensions.DependencyInjection;
using ProjectManagementAPI.Interfaces;
using ProjectManagementAPI.Services;
using ProjectManagementApp.UI.Interfaces;
using ProjectManagementApp.UI.ViewModels;
using ProjectManagementApp.UI.Services;
using System.Windows;

namespace ProjectManagementApp.UI
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IProject, ProjectService>(); 
            serviceCollection.AddSingleton<IEmployee, EmployeeService>(); 
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


