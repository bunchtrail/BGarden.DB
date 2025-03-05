# Скрипт для добавления образцов растений в базу данных
Write-Host "Добавление экземпляров растений в базу данных Ботанического сада..." -ForegroundColor Green
Write-Host "-----------------------------------------------------------" -ForegroundColor Green

# Запуск с явным указанием аргумента
dotnet run --project Infrastructure/Infrastructure.csproj -- --add-specimens

Write-Host "-----------------------------------------------------------" -ForegroundColor Green
Write-Host "Проверьте вывод программы для подтверждения успешного добавления экземпляров." -ForegroundColor Yellow
Read-Host "Нажмите Enter для выхода" 