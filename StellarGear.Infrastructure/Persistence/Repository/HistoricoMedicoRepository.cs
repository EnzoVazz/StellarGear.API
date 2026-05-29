using Microsoft.EntityFrameworkCore;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;

namespace StellarGear.Infrastructure.Persistence.Repositories;

public class HistoricoMedicoRepository : IHistoricoMedicoRepository
{
    private readonly StellarGearContext _context;

    public HistoricoMedicoRepository(StellarGearContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HistoricoMedico>> GetAllAsync()
    {
        return await _context.HistoricosMedicos.ToListAsync();
    }

    public async Task<HistoricoMedico?> GetByIdAsync(int id)
    {
        return await _context.HistoricosMedicos.FindAsync(id);
    }

    public async Task<HistoricoMedico> AddAsync(HistoricoMedico historico)
    {
        await _context.HistoricosMedicos.AddAsync(historico);
        await _context.SaveChangesAsync();
        return historico;
    }

    public async Task UpdateAsync(HistoricoMedico historico)
    {
        _context.HistoricosMedicos.Update(historico);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var historico = await _context.HistoricosMedicos.FindAsync(id);
        if (historico != null)
        {
            _context.HistoricosMedicos.Remove(historico);
            await _context.SaveChangesAsync();
        }
    }
}