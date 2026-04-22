using AppCore.Services;
using Infrastructure.Module;
using Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//  (AutoMapper, Walidatory)
builder.Services.AddContactsModule(builder.Configuration);

// (baza danych, Identity, repozytoria)
builder.Services.AddContactsEfModule(builder.Configuration);

builder.Services.AddSingleton<JwtSettings>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();