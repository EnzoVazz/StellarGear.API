using StellarGear.Domain.Entities;

namespace StellarGear.Application.Interfaces;

public interface IHistoricoMedicoRepository
{
    Task<IEnumerable<HistoricoMedico>> GetAllAsync();
    Task<HistoricoMedico?> GetByIdAsync(int id);
    Task<HistoricoMedico> AddAsync(HistoricoMedico historico);
    Task UpdateAsync(HistoricoMedico historico);
    Task DeleteAsync(int id);
}