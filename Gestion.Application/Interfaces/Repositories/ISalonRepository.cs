using Gestion.Domain.Models;

namespace Gestion.Application.Interfaces.Repositories
{
    public interface ISalonRepository
    {
        Task<IEnumerable<Salon>> GetAllAsync();
    }
}