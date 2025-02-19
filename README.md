Project Management App

Описание проекта
Десктопное приложение для управления проектами с трехуровневой архитектурой (DAL, BLL, UI) и Web API, взаимодействующим с базой данных PostgreSQL.

Структура проекта

ProjectManagementApp/
├── ProjectManagement.BLL/         # Бизнес-логика (сервисы, интерфейсы)
├── ProjectManagement.DAL/         # Уровень доступа к данным (Entity Framework, модели, подключение к БД)
├── ProjectManagementAPI/          # Web API (контроллеры, взаимодействие с БД)
├── ProjectManagementApp.UI/       # WPF UI (интерфейс пользователя, MVVM)
├── docker-compose.yml             # Docker Compose для контейнеризации
├── ProjectManagementApp.sln       # Файл решения Visual Studio
├── start_app.bat                  # Скрипт для запуска приложения

Используемые технологии

.NET 8.0
WPF (MVVM)
Entity Framework Core (Code First, автоматические миграции)
PostgreSQL 15
ASP.NET Core Web API
Docker/Docker Compose

Установка и запуск

1. Перейдите в папку ProjectManagementAPI\ProjectManagementApp.
2. Запустите приложение, открыв файл start_app.bat.
3. После запуска UI, перейдите по ссылке http://localhost:5225/swagger/index.html для доступа к Swagger интерфейсу Web API.

