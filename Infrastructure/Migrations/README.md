# Инструкции по применению миграций

## Предварительные требования

1. Установите инструменты Entity Framework Core:

```
dotnet tool install --global dotnet-ef
```

## Создание миграций

Для создания миграций выполните в корневой директории проекта:

```
dotnet ef migrations add ИмяМиграции --project Infrastructure --startup-project Infrastructure
```

Например:

```
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Infrastructure
```

## Применение миграций

Для применения миграций к базе данных:

```
dotnet ef database update --project Infrastructure --startup-project Infrastructure
```

## Удаление последней миграции

Если необходимо удалить последнюю созданную миграцию:

```
dotnet ef migrations remove --project Infrastructure --startup-project Infrastructure
```

## Сброс базы данных (Осторожно!)

Для удаления всех данных и пересоздания базы данных:

```
dotnet ef database drop --project Infrastructure --startup-project Infrastructure
```

## Генерация SQL скрипта

Для создания SQL скрипта миграции:

```
dotnet ef migrations script --project Infrastructure --startup-project Infrastructure
```
