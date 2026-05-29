using Microsoft.EntityFrameworkCore;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;

namespace StellarGear.Infrastructure.Persistence.Repositories;

public class PassageiroRepository : IPassageiroRepository
{
    private readonly StellarGearContext _context;

    public PassageiroRepository(StellarGearContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Passageiro>> GetAllAsync()
    {
        return await _context.Passageiros.ToListAsync();
    }

    public async Task<Passageiro?> GetByIdAsync(int id)
    {
        return await _context.Passageiros.FindAsync(id);
    }

    public async Task<Passageiro> AddAsync(Passageiro passageiro)
    {
        await _context.Passageiros.AddAsync(passageiro);
        await _context.SaveChangesAsync();
        return passageiro;
    }

    public async Task UpdateAsync(Passageiro passageiro)
    {
        _context.Passageiros.Update(passageiro);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var passageiro = await _context.Passageiros.FindAsync(id);
        if (passageiro != null)
        {
            _context.Passageiros.Remove(passageiro);
            await _context.SaveChangesAsync();
        }
    }
}