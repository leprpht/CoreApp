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
        var organization = _data.Values.FirstOrDefault(o => o.Id == organizationId);
        if (organization?.Members == null)
            return Task.FromResult(Enumerable.Empty<Person>());
        
        return Task.FromResult(organization.Members.AsEnumerable());
    }

    public async Task CreateAsync(Organization organization)
    {
        await AddAsync(organization);
    }

    public Task UpdateAsync(Organization organization)
    {
        if (_data.ContainsKey(organization.Id))
        {
            _data[organization.Id] = organization;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _data.Remove(id);
        return Task.CompletedTask;
    }
}