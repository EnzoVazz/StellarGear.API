using Microsoft.EntityFrameworkCore;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;

namespace StellarGear.Infrastructure.Persistence.Repositories;

public class MedicoRepository : IMedicoRepository
{
    private readonly StellarGearContext _context;

    public MedicoRepository(StellarGearContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Medico>> GetAllAsync()
    {
        return await _context.Medicos.ToListAsync();
    }

    public async Task<Medico?> GetByIdAsync(int id)
    {
        return await _context.Medicos.FindAsync(id);
    }

    public async Task<Medico> AddAsync(Medico medico)
    {
        await _context.Medicos.AddAsync(medico);
        await _context.SaveChangesAsync();
        return medico;
    }

    public async Task UpdateAsync(Medico medico)
    {
        _context.Medicos.Update(medico);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var medico = await _context.Medicos.FindAsync(id);
        if (medico != null)
        {
            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
        }
    }
}