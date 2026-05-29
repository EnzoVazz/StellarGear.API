using StellarGear.Domain.Entities;

namespace StellarGear.Application.Interfaces;

public interface ITrajeRepository
{
    Task<IEnumerable<Traje>> GetAllAsync();
    Task<Traje?> GetByIdAsync(int id);
    Task<Traje> AddAsync(Traje traje);
    Task UpdateAsync(Traje traje);
    Task DeleteAsync(int id);
}