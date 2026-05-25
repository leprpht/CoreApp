using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfPositionRepository(ContactsDbContext context)
    : EfGenericRepository<Position>(context.Positions), IPositionRepository
{
    public async Task<IEnumerable<Position>> GetAllAsync() =>
        await context.Positions.ToListAsync();

    public async Task CreateAsync(Position position)
    {
        await context.Positions.AddAsync(position);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Position position)
    {
        context.Positions.Update(position);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var position = await context.Positions.FindAsync(id);
        if (position != null)
        {
            context.Positions.Remove(position);
            await context.SaveChangesAsync();
        }
    }
}