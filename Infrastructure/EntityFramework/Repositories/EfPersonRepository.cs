using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfPersonRepository(ContactsDbContext context)
    : EfGenericRepository<Person>(context.People), IPersonRepository
{
    public override async Task<Person?> FindByIdAsync(Guid id) =>
        await WithIncludes()
            .FirstOrDefaultAsync(p => p.Id == id);

    public override async Task<PagedResult<Person>> FindPagedAsync(int page, int pageSize)
    {
        var query = WithIncludes().AsNoTracking();

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Person>(items, total, page, pageSize);
    }

    public async Task<IEnumerable<Person>> GetByEmployerAsync(Guid companyId) =>
        await WithIncludes()
            .Where(p => p.Employer != null && p.Employer.Id == companyId)
            .ToListAsync();

    public async Task<IEnumerable<Person>> GetByOrganizationAsync(Guid organizationId) =>
        await WithIncludes()
            .Where(p => p.Organization != null && p.Organization.Id == organizationId)
            .ToListAsync();

    private IQueryable<Person> WithIncludes() =>
        context.People
            .Include(p => p.Notes)
            .Include(p => p.Tags)
            .Include(p => p.Employer)
            .Include(p => p.Organization);
}