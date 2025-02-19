@echo off
echo Starting Docker containers...
docker-compose up -d
echo Checking if the file exists...
if exist "C:\Users\USER\source\repos\ProjectManagementAPI\ProjectManagementApp\ProjectManagementApp.UI\bin\Debug\net8.0-windows\ProjectManagementApp.UI.exe" (
    echo File found, starting the application...
    start "" "C:\Users\USER\source\repos\ProjectManagementAPI\ProjectManagementApp\ProjectManagementApp.UI\bin\Debug\net8.0-windows\ProjectManagementApp.UI.exe"
) else (
    echo Error: file not found.
)
echo All done!
pause





