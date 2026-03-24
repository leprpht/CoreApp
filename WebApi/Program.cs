using AppCore.Repositories;
using AppCore.Services;
using AutoMapper;
using Infrastructure.Memory;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Регистрация твоих хранилищ (Repositories)
builder.Services.AddSingleton<IPersonRepository, MemoryPersonRepository>();
builder.Services.AddSingleton<ICompanyRepository, MemoryCompanyRepository>();
builder.Services.AddSingleton<IOrganizationRepository, MemoryOrganizationRepository>();

// Регистрация Unit of Work
builder.Services.AddSingleton<IContactUnitOfWork, MemoryContactUnitOfWork>();

// Регистрация Сервиса
builder.Services.AddSingleton<IPersonService, MemoryPersonService>();
builder.Services.AddControllers();

// --- СЕКЦИЯ 2: СБОРКА ПРИЛОЖЕНИЯ (Один раз!) ---

var app = builder.Build();

// --- СЕКЦИЯ 3: НАСТРОЙКА ПРАВИЛ (Middleware) ---

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();