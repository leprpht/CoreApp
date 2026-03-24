using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryGenericRepository<T> : IGenericRepositoryAsync<T>
    where T : EntityBase
{
    protected Dictionary<Guid, T> _data = new();

    public Task<T?> FindByIdAsync(Guid id)
    {
        var result = _data
            .Where(t => t.Key == id)
            .Select(t => t.Value)
            .FirstOrDefault();
        return Task.FromResult(result);
    }

    public Task<IEnumerable<T>> FindAllAsync()
    {
        return Task.FromResult(_data.Values.AsEnumerable());
    }

    public Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        var items = _data.Values
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(new PagedResult<T>(
            items,
            _data.Count,
            page,
            pageSize
        ));
    }

    public Task<T> AddAsync(T entity)
    {
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(T entity)
    {
        if (!_data.ContainsKey(entity.Id))
            throw new KeyNotFoundException();

        _data[entity.Id] = entity;

        return Task.FromResult(entity);
    }

    public Task RemoveByIdAsync(Guid id)
    {
        var removed = _data.Remove(id);
        return removed ? Task.CompletedTask : throw new KeyNotFoundException();
    }
}