using AppCore.Entities;

namespace AppCore.Dto;

public abstract record ContactDtos
{
    public Guid Id { get; init; }
    protected string Email { get; init; } = string.Empty;
    protected string Phone { get; init; } = string.Empty;
    protected AddressDto? Address { get; init; } = null!;
    protected ContactStatus Status { get; init; }
    public List<string> Tags { get; init; } = new();
    public DateTime CreatedAt { get; init; }

    public static void MapBaseProperties(Contact entity, ContactDtos dto)
    {
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.Status = dto.Status;
        if (dto.Address != null)
            entity.Address = new Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                PostalCode = dto.Address.PostalCode,
                Country = dto.Address.Country,
                Type = dto.Address.Type
            };
    }
}

public record AddressDto(string Street, string City, string PostalCode, string Country, AddressType Type);