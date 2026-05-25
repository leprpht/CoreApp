using AppCore.Dto;
using AppCore.Entities;

namespace AppCore.Mapper;

public static class OrganizationMapper
{
    public static OrganizationDto MapToDto(this Organization organization)
    {
        return new OrganizationDto()
        {
            Id = organization.Id,
            Name = organization.Name,
            Type = organization.Type,
            KRS = organization.KRS,
            Website = organization.Website,
            Mission = organization.Mission,
        };
    }
}