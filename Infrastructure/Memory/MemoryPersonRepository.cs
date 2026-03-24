// Это наш «Специальный ящик для человечков». 
// Он умеет всё, что умеет обычный ящик, плюс пара секретов.

using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.Memory;

public class MemoryPersonRepository : MemoryGenericRepository<Person>, IPersonRepository
{
    public MemoryPersonRepository()
    {
        // Создаем первого игрушечного человечка по имени Адам
        var person1 = new Person { Id = Guid.NewGuid(), FirstName = "Adam", LastName = "Nowak" };

        // Создаем второго человечка по имени Ян
        var person2 = new Person { Id = Guid.NewGuid(), FirstName = "Jan", LastName = "Kowalski" };

        // Кладем их в наш ящик (_data), чтобы они там лежали
        _data[person1.Id] = person1;
        _data[person2.Id] = person2;
    }

    // Команда: «Найди всех, кто работает на эту компанию»
    public async Task<IEnumerable<Person>> GetByEmployerAsync(Guid companyId)
    {
        // Просим ящик: «Дай нам только тех, у кого в графе Работа написан нужный ID»
        var result = _data.Values
            .Where(p => p.Employer != null && p.Employer.Id == companyId)
            .ToList();

        return await Task.FromResult(result);
    }

    // Команда: «Найди всех, кто состоит в этой организации»
    public async Task<IEnumerable<Person>> GetByOrganizationAsync(Guid organizationId)
    {
        // Пока мы еще не научили человечков ходить в организации, 
        // поэтому просто отдаем пустой список (пустую ладошку)
        return await Task.FromResult(new List<Person>());
    }
}