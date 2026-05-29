using Microsoft.EntityFrameworkCore;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;

namespace StellarGear.Infrastructure.Persistence.Repositories;

public class LeituraSensorRepository : ILeituraSensorRepository
{
    private readonly StellarGearContext _context;

    public LeituraSensorRepository(StellarGearContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LeituraSensor>> GetAllAsync()
    {
        return await _context.LeiturasSensores.Include(l => l.Traje).ToListAsync();
    }

    public async Task<LeituraSensor?> GetByIdAsync(int id)
    {
        return await _context.LeiturasSensores.Include(l => l.Traje).FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<LeituraSensor> AddAsync(LeituraSensor leitura)
    {
        await _context.LeiturasSensores.AddAsync(leitura);
        await _context.SaveChangesAsync();
        return leitura;
    }

    public async Task UpdateAsync(LeituraSensor leitura)
    {
        _context.LeiturasSensores.Update(leitura);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var leitura = await _context.LeiturasSensores.FindAsync(id);
        if (leitura != null)
        {
            _context.LeiturasSensores.Remove(leitura);
            await _context.SaveChangesAsync();
        }
    }
}