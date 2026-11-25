using CharacterHub.Filters;
using CharacterHub.Repositories;
using CharacterHub.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<BusinessExceptionFilter>();
});

builder.Services.AddSingleton<IPersonajeRepository, JsonPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();

var app = builder.Build();

app.MapControllers();

app.Run();
