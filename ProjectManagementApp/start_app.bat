@echo off
echo Starting Docker containers...

:: Запуск Docker контейнеров в фоновом режиме
docker-compose up -d

:: Даем Docker время на инициализацию контейнеров (можно установить больше времени, если нужно)
echo Waiting for Docker containers to start...
timeout /t 10 /nobreak

:: Проверка, что контейнеры работают (по порту или статусу)
docker ps

echo Checking if the file exists...
if exist "C:\Users\USER\source\repos\ProjectManagementAPI\ProjectManagementApp\ProjectManagementApp.UI\bin\Debug\net8.0-windows\ProjectManagementApp.UI.exe" (
    echo File found, starting the application...
    start "" "C:\Users\USER\source\repos\ProjectManagementAPI\ProjectManagementApp\ProjectManagementApp.UI\bin\Debug\net8.0-windows\ProjectManagementApp.UI.exe"
) else (
    echo Error: file not found.
)

echo All done!
pause





