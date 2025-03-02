using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BGarden.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Специализированный репозиторий для работы с фенологическими данными (Phenology).
    /// </summary>
    public interface IPhenologyRepository : IRepository<Phenology>
    {
        Task<IEnumerable<Phenology>> GetBySpecimenIdAsync(int specimenId);
        Task<IEnumerable<Phenology>> GetByYearAsync(int year);
        Task<Phenology?> GetBySpecimenAndYearAsync(int specimenId, int year);
        Task<IEnumerable<Phenology>> GetByFloweringPeriodAsync(DateTime startDate, DateTime endDate);
    }
} 