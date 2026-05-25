using AppCore.Entities;

namespace AppCore.Repositories;

public interface ITagRepository
{
    Task<Tag?> FindByIdAsync(Guid id);
    Task<IEnumerable<Tag>> FindByNameAsync(string name);
    Task CreateAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task DeleteAsync(Guid id);
}