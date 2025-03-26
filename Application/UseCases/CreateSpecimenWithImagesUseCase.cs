using Application.DTO;
using Application.Mappers;
using BGarden.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases
{
    /// <summary>
    /// UseCase для создания нового образца (Specimen) с изображениями в одной транзакции.
    /// </summary>
    public class CreateSpecimenWithImagesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSpecimenWithImagesUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Создает образец растения с прикрепленными изображениями в одной транзакции
        /// </summary>
        /// <param name="specimenDto">Данные образца растения</param>
        /// <param name="images">Список изображений в бинарном формате</param>
        /// <returns>DTO созданного образца с идентификаторами прикрепленных изображений</returns>
        public async Task<(SpecimenDto Specimen, List<int> ImageIds)> ExecuteAsync(
            SpecimenDto specimenDto, 
            List<CreateSpecimenImageBinaryDto> images)
        {
            // Проверка входных данных
            if (specimenDto == null)
                throw new ArgumentNullException(nameof(specimenDto));
            
            if (images == null || images.Count == 0)
                throw new ArgumentException("Необходимо предоставить хотя бы одно изображение");

            // Начинаем транзакцию
            await _unitOfWork.BeginTransactionAsync();
            
            try
            {
                // 1. Создаем образец
                var specimenEntity = specimenDto.ToEntity();
                await _unitOfWork.Specimens.AddAsync(specimenEntity);
                await _unitOfWork.SaveChangesAsync();

                // 2. Получаем ID созданного образца
                int specimenId = specimenEntity.Id;
                
                // 3. Обновляем SpecimenId во всех DTO изображений
                foreach (var image in images)
                {
                    image.SpecimenId = specimenId;
                }
                
                // 4. Добавляем изображения
                var imageIds = new List<int>();
                foreach (var imageDto in images)
                {
                    var imageEntity = imageDto.ToEntity();
                    await _unitOfWork.SpecimenImages.AddAsync(imageEntity);
                    await _unitOfWork.SaveChangesAsync();
                    imageIds.Add(imageEntity.Id);
                }
                
                // 5. Фиксируем транзакцию
                await _unitOfWork.CommitTransactionAsync();
                
                // 6. Возвращаем DTO образца и список ID созданных изображений
                return (specimenEntity.ToDto(), imageIds);
            }
            catch (Exception)
            {
                // В случае ошибки откатываем транзакцию
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Создает образец растения с одним прикрепленным изображением в одной транзакции
        /// </summary>
        /// <param name="specimenDto">Данные образца растения</param>
        /// <param name="image">Изображение в бинарном формате</param>
        /// <returns>DTO созданного образца с идентификатором прикрепленного изображения</returns>
        public async Task<(SpecimenDto Specimen, int ImageId)> ExecuteAsync(
            SpecimenDto specimenDto, 
            CreateSpecimenImageBinaryDto image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var result = await ExecuteAsync(specimenDto, new List<CreateSpecimenImageBinaryDto> { image });
            return (result.Specimen, result.ImageIds[0]);
        }
    }
} 