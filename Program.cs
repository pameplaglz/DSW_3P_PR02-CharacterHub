using CharacterHub.Data;
using CharacterHub.Filters;
using CharacterHub.Repositories;
using CharacterHub.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<BusinessExceptionFilter>();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonajeRepository, EfPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();

var app = builder.Build();

app.MapControllers();

app.Run();
