// Это наш «Специальный ящик для человечков». 
// Он умеет всё, что умеет обычный ящик, плюс пара секретов.

using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.Memory;

public class MemoryPersonRepository : MemoryGenericRepository<Person>, IPersonRepository
{
    public MemoryPersonRepository()
    {
        var person1 = new Person { Id = Guid.NewGuid(), FirstName = "Adam", LastName = "Nowak" };
        var person2 = new Person { Id = Guid.NewGuid(), FirstName = "Jan", LastName = "Kowalski" };
        
        _data[person1.Id] = person1;
        _data[person2.Id] = person2;
    }
    
    public async Task<IEnumerable<Person>> GetByEmployerAsync(Guid companyId)
    {
        var result = _data.Values
            .Where(p => p.Employer != null && p.Employer.Id == companyId)
            .ToList();

        return await Task.FromResult(result);
    }
    
    public async Task<IEnumerable<Person>> GetByOrganizationAsync(Guid organizationId) => 
        await Task.FromResult(new List<Person>());
}