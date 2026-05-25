using AppCore.Entities;

namespace AppCore.Repositories;

public interface IPositionRepository
{
    Task<Position?> FindByIdAsync(Guid id);
    Task<IEnumerable<Position>> GetAllAsync();
    Task CreateAsync(Position position);
    Task UpdateAsync(Position position);
    Task DeleteAsync(Guid id);
}