using AppCore.Mapper;
using AppCore.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Scans the AppCore assembly for all AbstractValidator<T> implementations
        services.AddValidatorsFromAssemblyContaining<CreatePersonDtoValidator>();
        services.AddAutoMapper(typeof(ContactsMappingProfile).Assembly);

        return services;
    }
}