using AppCore.Mapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Регистрация всех валидаторов (чтобы проверки работали)
        services.AddValidatorsFromAssemblyContaining<CreatePersonDtoValidator>();

        // 2. Регистрация Автомаппера (чтобы превращение объектов работало)
        services.AddAutoMapper(typeof(ContactsMappingProfile).Assembly);

        return services;
    }
}