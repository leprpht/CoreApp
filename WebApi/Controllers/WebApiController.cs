using AppCore.Dto;
using AppCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreApp.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactsController(IPersonService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int size = 10)
    {
        var result = await service.FindAllPeoplePaged(page, size);
        return Ok(result); // Возвращает 200 OK и список людей в JSON
    }

    // Поиск человека по ID: 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var person = await service.FindById(id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        await service.CreatePerson(dto);
        return Created("api/contacts", dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonDto dto)
    {
        var person = await service.FindById(id);
        if (person == null) return NotFound();
        
        await service.UpdatePerson(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeletePerson(id);
        return NoContent();
    }
}