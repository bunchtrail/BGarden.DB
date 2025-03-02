# Репозитории и Unit of Work

В этой директории содержатся реализации паттернов Repository и Unit of Work, которые используются для доступа к данным в проекте BGarden.DB.

## Паттерн Repository

Паттерн Repository создает абстракцию между слоем доменной модели и слоем доступа к данным.
Он инкапсулирует логику получения и сохранения данных, что позволяет:

1. Скрыть сложность работы с ORM (EF Core) от остальных слоев приложения
2. Централизовать логику запросов к базе данных
3. Упростить тестирование, заменяя реальные репозитории mock-объектами

### Структура репозиториев

1. `IRepository<T>` - базовый интерфейс с общими операциями (GetById, GetAll, Add, Update, Remove)
2. `RepositoryBase<T>` - базовая реализация интерфейса
3. Специализированные репозитории (например, `ISpecimenRepository` и `SpecimenRepository`) для определенных сущностей, требующих особую логику

## Паттерн Unit of Work

Unit of Work обеспечивает координированную работу с несколькими репозиториями в рамках единой транзакции.
Этот паттерн гарантирует, что все изменения будут сохранены атомарно.

### Преимущества

1. Управление транзакциями
2. Синхронизация сохранения изменений между разными репозиториями
3. Повышение производительности за счет уменьшения числа обращений к БД

## Использование

```csharp
public class SampleService
{
    private readonly IUnitOfWork _unitOfWork;

    public SampleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task UpdateSpecimenInfoAsync(int specimenId, string newInfo)
    {
        var specimen = await _unitOfWork.Specimens.GetByIdAsync(specimenId);
        if (specimen != null)
        {
            specimen.Notes = newInfo;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
```

## Регистрация в DI

Регистрация репозиториев и Unit of Work в контейнере зависимостей осуществляется в классе `DependencyInjection.cs` с использованием метода расширения `AddInfrastructure`.
