using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;

namespace AppCore.Services;

public class PositionService(IPositionRepository positionRepository)
{
    public async Task<PositionDto> CreatePositionAsync(CreatePositionDto dto)
    {
        var position = new Position 
        { 
            Title = dto.Title,
            Description = dto.Description
        };
        await positionRepository.CreateAsync(position);
        return new PositionDto(position.Id, position.Title, position.Description);
    }

    public async Task<IEnumerable<PositionDto>> GetAllPositionsAsync()
    {
        var positions = await positionRepository.GetAllAsync();
        return positions.Select(p => new PositionDto(p.Id, p.Title, p.Description));
    }

    public async Task<PositionDto?> GetPositionByIdAsync(Guid id)
    {
        var position = await positionRepository.FindByIdAsync(id);
        return position != null ? new PositionDto(position.Id, position.Title, position.Description) : null;
    }

    public async Task<PositionDto> UpdatePositionAsync(Guid id, CreatePositionDto dto)
    {
        var position = await positionRepository.FindByIdAsync(id);
        if (position == null) throw new InvalidOperationException("Position not found");
        
        position.Title = dto.Title;
        position.Description = dto.Description;
        await positionRepository.UpdateAsync(position);
        return new PositionDto(position.Id, position.Title, position.Description);
    }

    public async Task DeletePositionAsync(Guid id)
    {
        var position = await positionRepository.FindByIdAsync(id);
        if (position == null) throw new InvalidOperationException("Position not found");
        await positionRepository.DeleteAsync(id);
    }
}