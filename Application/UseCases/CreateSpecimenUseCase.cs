using Application.DTO;
using Application.Mappers;
using BGarden.Domain.Interfaces;

namespace Application.UseCases
{
    /// <summary>
    /// UseCase для создания нового образца (Specimen).
    /// Пример реализации в стиле Clean Architecture, где каждая бизнес-операция - отдельный класс.
    /// </summary>
    public class CreateSpecimenUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSpecimenUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Выполнить UseCase - создать новый образец (Specimen)
        /// </summary>
        /// <param name="dto">Данные для создания нового образца</param>
        /// <returns>DTO с данными созданного образца (включая Id)</returns>
        public async Task<SpecimenDto> ExecuteAsync(SpecimenDto dto)
        {
            // Можно добавить валидацию, бизнес-правила и т.д.
            // ValidateSpecimen(dto);
            
            // Преобразуем DTO в доменную сущность
            var entity = dto.ToEntity();
            
            // Добавляем в репозиторий через UnitOfWork
            await _unitOfWork.Specimens.AddAsync(entity);
            
            // Сохраняем изменения через UnitOfWork
            await _unitOfWork.SaveChangesAsync();
            
            // Возвращаем DTO с присвоенным Id
            return entity.ToDto();
        }
        
        // Пример метода валидации (реальный код будет зависеть от бизнес-правил)
        private void ValidateSpecimen(SpecimenDto dto)
        {
            if (string.IsNullOrEmpty(dto.InventoryNumber))
            {
                throw new ArgumentException("Инвентарный номер обязателен");
            }
            
            // Другие правила валидации...
        }
    }
} 