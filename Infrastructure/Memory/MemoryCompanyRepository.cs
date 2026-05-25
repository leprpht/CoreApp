using AppCore.Entities;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryCompanyRepository : MemoryGenericRepository<Company>, ICompanyRepository
{
    public Task<Company?> GetByNipAsync(string nip)
    {
        var company = _data.Values.FirstOrDefault(c => c.Nip == nip);
        return Task.FromResult(company);
    }
    
    public Task<IEnumerable<Company>> FindByNameAsync(string name)
    {
        var companies = _data.Values.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(companies);
    }

    public Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {
        var company = _data.Values.FirstOrDefault(c => c.Id == companyId);
        if (company?.Employees == null)
            return Task.FromResult(Enumerable.Empty<Person>());
        
        return Task.FromResult(company.Employees.AsEnumerable());
    }
}