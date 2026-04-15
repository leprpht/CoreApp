using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfOrganizationRepository(ContactsDbContext context)
    : EfGenericRepository<Organization>(context.Organizations), IOrganizationRepository
{
    public async Task<IEnumerable<Organization>> GetByTypeAsync(OrganizationType type) =>
        await context.Organizations
            .Where(o => o.Type == type)
            .ToListAsync();

    public async Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId) =>
        await context.People
            .Where(p => p.Organization != null && p.Organization.Id == organizationId)
            .ToListAsync();
}