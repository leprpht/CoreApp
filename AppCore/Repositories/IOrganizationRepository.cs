using AppCore.Entities;

namespace AppCore.Repositories;

public interface IOrganizationRepository
{
    Task<Organization?> FindByIdAsync(Guid id);
    Task CreateAsync(Organization organization);
    Task UpdateAsync(Organization organization);
    Task DeleteAsync(Guid id);
}