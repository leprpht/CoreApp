namespace AppCore.Dto;

public record PositionDto(Guid Id, string Title);

public record CreatePositionDto(string Title);