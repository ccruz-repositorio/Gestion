using Gestion.Application.DTOs;
using Gestion.Application.Enums;
using Gestion.Domain.Models;

namespace Gestion.Application.Interfaces.Services
{

    public interface IReservaService
    {
        Task<(ResponseCode code, string message, IEnumerable<Reserva>? data)> GetReservasPorFechaAsync(DateTime fecha);
        Task<(ResponseCode code, string message)> CrearReservaAsync(ReservaDto nuevaReservaDto);
    }
}