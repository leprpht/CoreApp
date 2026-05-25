using AppCore.Entities;

namespace AppCore.Dto;

public record OrganizationDto : ContactDtos
{
    public string Name { get; init; } = string.Empty;
    public OrganizationType Type { get; init; }
    public string? KRS { get; init; }
    public string? Website { get; init; }
    public string? Mission { get; init; }
    public List<NoteDto> Notes { get; init; } = new();

    public static OrganizationDto FromEntity(Organization org) => new()
    {
        Id = org.Id,
        Name = org.Name,
        Type = org.Type,
        KRS = org.KRS,
        Website = org.Website,
        Mission = org.Mission,
        Email = org.Email,
        Phone = org.Phone,
        Status = org.Status,
        CreatedAt = org.CreatedAt,
        Tags = org.Tags?.Select(t => t.Name).ToList() ?? [],
        Notes = org.Notes?.Select(NoteDto.FromEntity).ToList() ?? [],
        Address = org.Address != null
            ? new AddressDto(org.Address.Street, org.Address.City, org.Address.PostalCode, org.Address.Country, org.Address.Type)
            : null
    };
}

public record UpdateOrganizationDto(
    string? Name,
    OrganizationType? Type,
    string? KRS,
    string? Website,
    string? Mission,
    string? Email,
    string? Phone,
    AddressDto? Address,
    ContactStatus? Status);