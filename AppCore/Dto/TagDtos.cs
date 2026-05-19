namespace AppCore.Dto;

public record TagDto(Guid Id, string Name, string Color);

public record CreateTagDto(string Name, string Color = "#3B82F6");