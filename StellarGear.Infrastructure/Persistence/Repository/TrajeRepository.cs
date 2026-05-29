using Microsoft.EntityFrameworkCore;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;

namespace StellarGear.Infrastructure.Persistence.Repositories;

public class TrajeRepository : ITrajeRepository
{
    private readonly StellarGearContext _context;

    public TrajeRepository(StellarGearContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Traje>> GetAllAsync()
    {
        return await _context.Trajes.Include(t => t.Passageiro).ToListAsync();
    }

    public async Task<Traje?> GetByIdAsync(int id)
    {
        return await _context.Trajes.Include(t => t.Passageiro).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Traje> AddAsync(Traje traje)
    {
        await _context.Trajes.AddAsync(traje);
        await _context.SaveChangesAsync();
        return traje;
    }

    public async Task UpdateAsync(Traje traje)
    {
        _context.Trajes.Update(traje);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var traje = await _context.Trajes.FindAsync(id);
        if (traje != null)
        {
            _context.Trajes.Remove(traje);
            await _context.SaveChangesAsync();
        }
    }
}