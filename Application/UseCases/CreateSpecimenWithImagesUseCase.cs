using Application.DTO;
using Application.Mappers;
using BGarden.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO; // Для Path
using System.Linq; // Для Select и Any
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Application.Interfaces; // Для IFormFile

namespace Application.UseCases
{
    /// <summary>
    /// UseCase для создания нового образца (Specimen) с изображениями в одной транзакции.
    /// </summary>
    public class CreateSpecimenWithImagesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpecimenImageService _specimenImageService;

        public CreateSpecimenWithImagesUseCase(IUnitOfWork unitOfWork, ISpecimenImageService specimenImageService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _specimenImageService = specimenImageService ?? throw new ArgumentNullException(nameof(specimenImageService));
        }

        /// <summary>
        /// Создает образец растения с прикрепленными изображениями в одной транзакции.
        /// Файлы сохраняются в файловую систему (пока имитация).
        /// </summary>
        /// <param name="specimenDto">Данные образца растения</param>
        /// <param name="imageFiles">Коллекция файлов изображений (IFormFile)</param>
        /// <returns>DTO созданного образца с идентификаторами прикрепленных изображений</returns>
        public async Task<(SpecimenDto Specimen, List<int> ImageIds)> ExecuteAsync(
            SpecimenDto specimenDto,
            IEnumerable<IFormFile> imageFiles)
        {
            if (specimenDto == null)
                throw new ArgumentNullException(nameof(specimenDto));
            if (imageFiles == null || !imageFiles.Any())
                throw new ArgumentException("Необходимо предоставить хотя бы одно изображение.", nameof(imageFiles));

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. Создаем образец (Specimen)
                var specimenEntity = specimenDto.ToEntity();
                specimenEntity.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.Specimens.AddAsync(specimenEntity);
                await _unitOfWork.SaveChangesAsync(); // Сохраняем образец, чтобы получить его ID

                int specimenId = specimenEntity.Id;
                var imageIds = new List<int>();
                bool isFirstImage = true;

                // 2. Сохраняем каждый файл и создаем для него запись SpecimenImage через сервис
                foreach (var imageFile in imageFiles)
                {
                    if (imageFile == null || imageFile.Length == 0)
                        continue;

                    // Вызываем сервис для загрузки и добавления изображения
                    // Сервис сам сохранит файл и создаст запись в БД (но без SaveChanges)
                    var addedImageDto = await _specimenImageService.UploadAndAddImageAsync(
                        specimenId,
                        imageFile,
                        description: $"Загружен: {Path.GetFileName(imageFile.FileName)}", // Используем Path.GetFileName
                        isMain: isFirstImage
                    );
                    imageIds.Add(addedImageDto.Id);
                    isFirstImage = false;
                }
                
                // Все сущности SpecimenImage уже добавлены в контекст через _specimenImageService,
                // который вызывает _unitOfWork.SpecimenImages.AddAsync().
                // Теперь нужно сохранить эти изменения.
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return (specimenEntity.ToDto(), imageIds); // Используем SpecimenMapper
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                // TODO: Удалить физически сохраненные файлы, если транзакция БД не удалась.
                // Это можно сделать, собирая пути к сохраненным файлам в цикле и удаляя их в catch.
                Console.WriteLine($"[UseCase-Error] Ошибка при создании образца с изображениями: {ex.Message}. Начался откат.");
                throw;
            }
        }

        // Перегрузка для одного изображения (для удобства)
        public async Task<(SpecimenDto Specimen, int ImageId)> ExecuteAsync(
            SpecimenDto specimenDto,
            IFormFile imageFile)
        {
            if (imageFile == null)
                throw new ArgumentNullException(nameof(imageFile));

            var result = await ExecuteAsync(specimenDto, new List<IFormFile> { imageFile });
            return (result.Specimen, result.ImageIds.FirstOrDefault());
        }
    }
} 