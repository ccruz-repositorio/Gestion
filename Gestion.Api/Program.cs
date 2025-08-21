using Gestion.Application.Interfaces.Repositories;
using Gestion.Application.Interfaces.Services;
using Gestion.Application.Services;
using Gestion.Application.UnitOfWork;
using Gestion.Infrastructure.Context;
using Gestion.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar la configuración de Entity Framework Core con SQLite
builder.Services.AddDbContext<GestionDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar la inyección de dependencias para UnitOfWork
// La implementación de UnitOfWork se encargará de crear los repositorios internamente.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Agrego la inyeccion de dependencias para los servicios
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