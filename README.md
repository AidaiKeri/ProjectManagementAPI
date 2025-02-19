Project Management App

Описание проекта
Десктопное приложение для управления проектами с трехуровневой архитектурой (DAL, BLL, UI) и Web API, взаимодействующим с базой данных PostgreSQL.
Структура проекта

ProjectManagementApp/
├── .vs/                           # Файлы Visual Studio
├── ProjectManagement.BLL/         # Бизнес-логика (сервисы, интерфейсы)
├── ProjectManagement.DAL/         # Уровень доступа к данным (Entity Framework, модели, подключение к БД)
├── ProjectManagementAPI/          # Web API (контроллеры, взаимодействие с БД)
├── ProjectManagementApp.UI/       # WPF UI (интерфейс пользователя, MVVM)
├── publish/                       # Сборка приложения
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

1. Запуск через Docker

Убедитесь, что у вас установлен Docker и Docker Compose.
Открыть терминал/командную строку в корневой папке проекта.
Запустить контейнеры командой:
docker-compose up --build
Это поднимет контейнеры с API и базой данных PostgreSQL.

2. Запуск вручную

Настройка базы данных
Убедитесь, что PostgreSQL установлен и запущен.
Создайте базу данных ProjectApi с параметрами:
Сервер: localhost
Порт: 5430
Пользователь: postgres
Пароль: 1123581321

Запуск WPF приложения
Перейти в папку ProjectManagementApp.UI/.
Открыть проект в Visual Studio.
Собрать и запустить ProjectManagementApp.UI.
Или запустить через start_app.bat.
