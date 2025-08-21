using Gestion.Application.Interfaces.Repositories;
using Gestion.Domain.Models;
using Gestion.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Gestion.Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly GestionDbContext _context;

        public ReservaRepository(GestionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reserva>> GetByFechaAsync(DateTime fecha)
        {
            return await _context.Reserva
                                .Where(r => r.FechaReserva.Date == fecha.Date)
                                .ToListAsync();
        }

        public async Task AddAsync(Reserva reserva)
        {
            await _context.Reserva.AddAsync(reserva);
            //await _context.SaveChangesAsync();
        }
    }
}