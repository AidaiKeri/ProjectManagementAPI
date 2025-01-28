using Microsoft.Extensions.DependencyInjection;
using ProjectManagementAPI.Interfaces;
using ProjectManagementAPI.Services;
using ProjectManagementApp.UI.Interfaces;
using ProjectManagementApp.UI.Services;
using System.Windows;

namespace ProjectManagementApp.UI
{
    public partial class App : Application
    {
        // DI контейнер
        public static ServiceProvider ServiceProvider { get; private set; }

        // Инициализация DI контейнера
        public App()
        {
            var serviceCollection = new ServiceCollection();

            // Регистрируем сервисы
            serviceCollection.AddSingleton<IProject, ProjectService>(); // Регистрация ProjectService
            serviceCollection.AddSingleton<IEmployee, EmployeeService>(); // Регистрация EmployeeService
            serviceCollection.AddSingleton<IWindowService, WindowService>();

            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<MainWindow>();

            // Создаем ServiceProvider
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        // Запуск приложения
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Получаем MainWindow с DI
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

            // Показываем окно
            mainWindow.Show();
        }
    }
}


