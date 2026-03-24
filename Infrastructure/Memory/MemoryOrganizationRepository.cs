using AppCore.Entities;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryOrganizationRepository : MemoryGenericRepository<Organization>, IOrganizationRepository
{
    public Task<IEnumerable<Organization>> GetByTypeAsync(OrganizationType type)
    {
        var orgs = _data.Values.Where(o => o.Type == type);
        return Task.FromResult(orgs);
    }

    public Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId)
    {
        throw new NotImplementedException();
    }
}