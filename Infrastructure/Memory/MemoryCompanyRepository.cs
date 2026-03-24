using AppCore.Entities;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryCompanyRepository : MemoryGenericRepository<Company>, ICompanyRepository
{
    // Реализуем метод поиска по NIP (ИНН)
    public Task<Company?> GetByNipAsync(string nip)
    {
        var company = _data.Values.FirstOrDefault(c => c.Nip == nip);
        return Task.FromResult(company);
    }

    // Поиск по названию
    public Task<IEnumerable<Company>> FindByNameAsync(string name)
    {
        var companies = _data.Values.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(companies);
    }

    // Получение списка сотрудников компании
    public Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {
        // В реальной базе это был бы JOIN, здесь просто пример:
        throw new NotImplementedException("Для этого метода нужен доступ к репозиторию людей");
    }
}