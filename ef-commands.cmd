@echo off
setlocal

echo BGarden.DB - Инструменты Entity Framework Core
echo =========================================
echo.
echo Выберите операцию:
echo 1 - Создать миграцию
echo 2 - Применить миграции
echo 3 - Удалить последнюю миграцию
echo 4 - Сбросить базу данных
echo 5 - Создать SQL-скрипт миграции
echo Q - Выход
echo.

set /p choice=Ваш выбор: 

if "%choice%"=="1" goto CreateMigration
if "%choice%"=="2" goto UpdateDatabase
if "%choice%"=="3" goto RemoveMigration
if "%choice%"=="4" goto DropDatabase
if "%choice%"=="5" goto ScriptMigration
if /i "%choice%"=="Q" goto End

echo Неверный выбор, попробуйте снова.
goto End

:CreateMigration
echo.
set /p migrationName=Введите имя миграции: 
dotnet ef migrations add %migrationName% --project Infrastructure --startup-project Infrastructure
goto End

:UpdateDatabase
echo.
dotnet ef database update --project Infrastructure --startup-project Infrastructure
goto End

:RemoveMigration
echo.
dotnet ef migrations remove --project Infrastructure --startup-project Infrastructure
goto End

:DropDatabase
echo.
echo ВНИМАНИЕ: Эта операция удалит все данные из базы данных.
set /p confirm=Вы уверены? (Y/N): 
if /i "%confirm%"=="Y" (
    dotnet ef database drop --project Infrastructure --startup-project Infrastructure
)
goto End

:ScriptMigration
echo.
set /p from=От миграции (пусто для начала): 
set /p to=До миграции (пусто для последней): 
set /p output=Имя выходного файла (migrations.sql): 

if "%output%"=="" set output=migrations.sql

if "%from%"=="" (
    if "%to%"=="" (
        dotnet ef migrations script --output %output% --project Infrastructure --startup-project Infrastructure
    ) else (
        dotnet ef migrations script --to %to% --output %output% --project Infrastructure --startup-project Infrastructure
    )
) else (
    if "%to%"=="" (
        dotnet ef migrations script %from% --output %output% --project Infrastructure --startup-project Infrastructure
    ) else (
        dotnet ef migrations script %from% %to% --output %output% --project Infrastructure --startup-project Infrastructure
    )
)
goto End

:End
echo.
echo Выполнение завершено.
endlocal 