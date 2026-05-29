using StellarGear.Domain.Entities;

namespace StellarGear.Application.Interfaces;

public interface ILeituraSensorRepository
{
    Task<IEnumerable<LeituraSensor>> GetAllAsync();
    Task<LeituraSensor?> GetByIdAsync(int id);
    Task<LeituraSensor> AddAsync(LeituraSensor leitura);
    Task UpdateAsync(LeituraSensor leitura);
    Task DeleteAsync(int id);
}