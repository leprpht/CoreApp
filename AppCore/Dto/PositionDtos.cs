namespace AppCore.Dto;

public record PositionDto(
    Guid Id,
    string Title,
    string? Description
);

public record CreatePositionDto(
    string Title,
    string? Description
);