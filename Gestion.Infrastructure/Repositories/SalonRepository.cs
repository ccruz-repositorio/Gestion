using Gestion.Application.Interfaces;
using Gestion.Application.Interfaces.Repositories;
using Gestion.Domain.Models;
using Gestion.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Gestion.Infrastructure.Repositories
{
    public class SalonRepository : ISalonRepository
    {
        private readonly GestionDbContext _context;

        public SalonRepository(GestionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Salon>> GetAllAsync()
        {
            return await _context.Salon.ToListAsync();
        }
    }
}