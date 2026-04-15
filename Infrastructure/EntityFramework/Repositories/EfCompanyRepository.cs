using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCompanyRepository(ContactsDbContext context)
    : EfGenericRepository<Company>(context.Companies), ICompanyRepository
{
    public async Task<Company?> GetByNipAsync(string nip) =>
        await context.Companies.FirstOrDefaultAsync(c => c.Nip == nip);

    public async Task<IEnumerable<Company>> FindByNameAsync(string name) =>
        await context.Companies
            .Where(c => c.Name.Contains(name))
            .ToListAsync();

    public async Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId) =>
        await context.People
            .Where(p => p.Employer != null && p.Employer.Id == companyId)
            .ToListAsync();
}