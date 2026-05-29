using StellarGear.Domain.Entities;

namespace StellarGear.Application.Interfaces;

public interface IMedicoRepository
{
    Task<IEnumerable<Medico>> GetAllAsync();
    Task<Medico?> GetByIdAsync(int id);
    Task<Medico> AddAsync(Medico medico);
    Task UpdateAsync(Medico medico);
    Task DeleteAsync(int id);
}