using Gestion.Application.Interfaces.Repositories;

namespace Gestion.Application.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IReservaRepository Reserva { get; }
        ISalonRepository Salon { get; }
        Task<int> CompleteAsync();
    }
}