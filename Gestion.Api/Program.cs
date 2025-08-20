using Gestion.Application.Interfaces.Repositories;
using Gestion.Application.Interfaces.Services;
using Gestion.Application.Services;
using Gestion.Infrastructure.Context;
using Gestion.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar la configuración de Entity Framework Core con SQLite
builder.Services.AddDbContext<GestionDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar la inyección de dependencias para los repos
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<ISalonRepository, SalonRepository>();

// Agrego la inyeccion de dependecias para los servicés
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<ISalonService, SalonService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();