using AppCore.Entities;

namespace AppCore.Dto;

public record CreateOrganizationDto(
    string Name,
    OrganizationType Type,
    string? Krs,
    string? Website
);