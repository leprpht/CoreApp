using AppCore.Dto;
using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.Repositories;
using AppCore.Services;
using AutoMapper;

namespace Infrastructure.Services;

public class PersonService(IContactUnitOfWork unitOfWork, IMapper mapper) : IPersonService
{
    public async Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size)
    {
        var result = await unitOfWork.Persons.FindPagedAsync(page, size);
        var dtos = result.Items.Select(PersonDto.FromEntity).ToList();
        return new PagedResult<PersonDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<PersonDto?> FindById(Guid id)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id);
        return person == null ? null : PersonDto.FromEntity(person);
    }

    public async Task<PersonDto> GetPerson(Guid personId)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId)
            ?? throw new ContactNotFoundException($"Person with id={personId} not found!");
        return PersonDto.FromEntity(person);
    }

    public async Task CreatePerson(CreatePersonDto dto)
    {
        var person = mapper.Map<Person>(dto);
        await unitOfWork.Persons.AddAsync(person);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdatePerson(Guid id, UpdatePersonDto dto)
    {
        var person = mapper.Map<Person>(dto);
        person.Id = id;
        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeletePerson(Guid id)
    {
        await unitOfWork.Persons.RemoveByIdAsync(id);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<Note> AddNoteToPerson(Guid personId, CreateNoteDto noteDto)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId)
            ?? throw new ContactNotFoundException($"Person with id={personId} not found!");

        var note = new Note
        {
            Content = noteDto.Content,
            CreatedBy = noteDto.CreatedBy ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        person.Notes.Add(note);
        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
        return note;
    }

    public async Task DeleteNoteFromPerson(Guid personId, Guid noteId)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId)
            ?? throw new ContactNotFoundException($"Person with id={personId} not found!");

        var note = person.Notes.FirstOrDefault(n => n.Id == noteId)
            ?? throw new NoteNotFoundException($"Note id={noteId} not found!");

        person.Notes.Remove(note);
        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }
}