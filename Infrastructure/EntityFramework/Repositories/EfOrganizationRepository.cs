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

    public async Task CreateAsync(Organization organization)
    {
        await context.Organizations.AddAsync(organization);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Organization organization)
    {
        context.Organizations.Update(organization);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var organization = await context.Organizations.FindAsync(id);
        if (organization != null)
        {
            context.Organizations.Remove(organization);
            await context.SaveChangesAsync();
        }
    }
}