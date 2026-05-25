using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;
using AppCore.Exceptions;

namespace AppCore.Services;

public class TagService(ITagRepository tagRepository)
{
    public async Task<TagDto> CreateTagAsync(CreateTagDto dto)
    {
        var tag = new Tag { Name = dto.Name, Color = dto.Color };
        await tagRepository.CreateAsync(tag);
        return new TagDto(tag.Id, tag.Name, tag.Color);
    }

    public async Task<TagDto> UpdateTagAsync(Guid id, CreateTagDto dto)
    {
        var tag = await tagRepository.FindByIdAsync(id);
        if (tag == null) throw new TagNotFoundException("Tag not found");
        
        tag.Name = dto.Name;
        tag.Color = dto.Color;
        await tagRepository.UpdateAsync(tag);
        return new TagDto(tag.Id, tag.Name, tag.Color);
    }

    public async Task DeleteTagAsync(Guid id)
    {
        var tag = await tagRepository.FindByIdAsync(id);
        if (tag == null) throw new TagNotFoundException("Tag not found");
        await tagRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var tags = await tagRepository.GetAllAsync();
        return tags.Select(t => new TagDto(t.Id, t.Name, t.Color));
    }

    public async Task<TagDto?> GetTagByIdAsync(Guid id)
    {
        var tag = await tagRepository.FindByIdAsync(id);
        return tag != null ? new TagDto(tag.Id, tag.Name, tag.Color) : null;
    }
}