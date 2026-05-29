using Microsoft.EntityFrameworkCore;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;

namespace StellarGear.Infrastructure.Persistence.Repositories;

public class AlertaEmergenciaRepository : IAlertaEmergenciaRepository
{
    private readonly StellarGearContext _context;

    public AlertaEmergenciaRepository(StellarGearContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AlertaEmergencia>> GetAllAsync()
    {
        return await _context.AlertasEmergencia.ToListAsync();
    }

    public async Task<AlertaEmergencia?> GetByIdAsync(int id)
    {
        return await _context.AlertasEmergencia.FindAsync(id);
    }

    public async Task<AlertaEmergencia> AddAsync(AlertaEmergencia alerta)
    {
        await _context.AlertasEmergencia.AddAsync(alerta);
        await _context.SaveChangesAsync();
        return alerta;
    }

    public async Task UpdateAsync(AlertaEmergencia alerta)
    {
        _context.AlertasEmergencia.Update(alerta);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var alerta = await _context.AlertasEmergencia.FindAsync(id);
        if (alerta != null)
        {
            _context.AlertasEmergencia.Remove(alerta);
            await _context.SaveChangesAsync();
        }
    }
}