using StellarGear.Domain.Entities;

namespace StellarGear.Application.Interfaces;

public interface IPassageiroRepository
{
    Task<IEnumerable<Passageiro>> GetAllAsync();
    Task<Passageiro?> GetByIdAsync(int id);
    Task<Passageiro> AddAsync(Passageiro passageiro);
    Task UpdateAsync(Passageiro passageiro);
    Task DeleteAsync(int id);
}