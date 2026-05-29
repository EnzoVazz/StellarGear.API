using StellarGear.Domain.Entities;

namespace StellarGear.Application.Interfaces;

public interface IAlertaEmergenciaRepository
{
    Task<IEnumerable<AlertaEmergencia>> GetAllAsync();
    Task<AlertaEmergencia?> GetByIdAsync(int id);
    Task<AlertaEmergencia> AddAsync(AlertaEmergencia alerta);
    Task UpdateAsync(AlertaEmergencia alerta);
    Task DeleteAsync(int id);
}