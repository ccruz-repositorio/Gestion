using Gestion.Domain.Models;

namespace Gestion.Application.Interfaces.Repositories
{
    public interface IReservaRepository
    {
        Task<IEnumerable<Reserva>> GetByFechaAsync(DateTime fecha);
        Task AddAsync(Reserva reserva);

    }
}