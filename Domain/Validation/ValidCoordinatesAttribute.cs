using System.ComponentModel.DataAnnotations;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;

namespace BGarden.Domain.Validation
{
    /// <summary>
    /// Атрибут для валидации координат образца
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidCoordinatesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var specimen = (Specimen)value;
            
            switch (specimen.LocationType)
            {
                case LocationType.Geographic:
                    if (!specimen.Latitude.HasValue || !specimen.Longitude.HasValue)
                        return new ValidationResult("При использовании географических координат должны быть указаны широта и долгота");
                    if (specimen.MapId.HasValue || specimen.MapX.HasValue || specimen.MapY.HasValue)
                        return new ValidationResult("При использовании географических координат не должны быть указаны координаты на схеме");
                    break;
                    
                case LocationType.SchematicMap:
                    if (!specimen.MapId.HasValue || !specimen.MapX.HasValue || !specimen.MapY.HasValue)
                        return new ValidationResult("При использовании схематической карты должны быть указаны идентификатор карты и координаты X, Y");
                    if (specimen.Latitude.HasValue || specimen.Longitude.HasValue || specimen.Location != null)
                        return new ValidationResult("При использовании схематической карты не должны быть указаны географические координаты");
                    break;
                    
                case LocationType.None:
                    if (specimen.Latitude.HasValue || specimen.Longitude.HasValue || specimen.Location != null || 
                        specimen.MapId.HasValue || specimen.MapX.HasValue || specimen.MapY.HasValue)
                        return new ValidationResult("Если тип координат не указан, все координаты должны быть пустыми");
                    break;
            }
            
            return ValidationResult.Success;
        }
    }
} 