@echo off
chcp 65001 > nul
echo Инициализация базы данных ботанического сада начальными значениями...
echo.

cd /d "%~dp0"
dotnet run --project Infrastructure/Infrastructure.csproj --framework net8.0 -- -initialize-db

echo.
echo Проверьте вывод программы выше для подтверждения успешной инициализации.
echo.
pause 