using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;
using AppCore.Services;
using AutoMapper;

namespace Infrastructure.Services;

public class MemoryPersonService(IContactUnitOfWork unitOfWork, IMapper mapper) : IPersonService
{
    public async Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size)
    {
        var result = await unitOfWork.Persons.FindPagedAsync(page, size);
        var dtos = result.Items.Select(PersonDto.FromEntity).ToList();
        return new PagedResult<PersonDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }

    // Остальные методы  просто добавь пустые с throw new NotImplementedException()
    public async Task<PersonDto?> FindById(Guid id)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id);
        return person == null ? null : PersonDto.FromEntity(person);
    }

    public async Task CreatePerson(CreatePersonDto dto)
    {
        var person = mapper.Map<CreatePersonDto, Person>(dto);
        await unitOfWork.Persons.AddAsync(person);
    }

    public async Task UpdatePerson(Guid id, UpdatePersonDto dto)
    {
        var person = mapper.Map<UpdatePersonDto, Person>(dto);
        person.Id = id;
        await unitOfWork.Persons.AddAsync(person);
    }

    public async Task DeletePerson(Guid id)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id);
        
        if (person != null)
            await unitOfWork.Persons.RemoveByIdAsync(person.Id);
    }
}