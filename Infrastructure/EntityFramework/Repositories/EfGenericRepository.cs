using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfGenericRepository<T>(DbSet<T> set) : IGenericRepositoryAsync<T>
    where T : EntityBase
{
    public virtual async Task<T?> FindByIdAsync(Guid id) =>
        await set.FindAsync(id);

    public async Task<IEnumerable<T>> FindAllAsync() =>
        await set.ToListAsync();

    public virtual async Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        var query = set.AsNoTracking();

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>(items, total, page, pageSize);
    }

    public async Task<T> AddAsync(T entity)
    {
        var entry = await set.AddAsync(entity);
        return entry.Entity;
    }

    public Task<T> UpdateAsync(T entity)
    {
        var entry = set.Update(entity);
        return Task.FromResult(entry.Entity);
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        var entity = await set.FindAsync(id);
        if (entity is not null)
            set.Remove(entity);
    }
}