using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BGarden.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Специализированный репозиторий для работы с биометрическими данными (Biometry).
    /// </summary>
    public interface IBiometryRepository : IRepository<Biometry>
    {
        Task<IEnumerable<Biometry>> GetBySpecimenIdAsync(int specimenId);
        Task<IEnumerable<Biometry>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Biometry>> GetLatestForSpecimenAsync(int specimenId, int count = 1);
    }
} 