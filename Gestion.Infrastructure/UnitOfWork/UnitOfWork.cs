using Gestion.Application.Interfaces.Repositories;
using Gestion.Application.UnitOfWork;
using Gestion.Infrastructure.Context;
using Gestion.Infrastructure.Repositories; // Asegúrate de que tus repositorios concretos estén aquí
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Gestion.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GestionDbContext _context;
        private IDbContextTransaction _currentTransaction;

        public IReservaRepository Reserva { get; }
        public ISalonRepository Salon { get; }

        public UnitOfWork(GestionDbContext context)
        {
            _context = context;
            Reserva = new ReservaRepository(_context);
            Salon = new SalonRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Métodos de transacción ahora son privados o internos
        // para que solo se utilicen dentro de la capa de Infrastructure
        private async Task BeginTransactionAsync()
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        private async Task CommitTransactionAsync()
        {
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        private async Task RollbackTransactionAsync()
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}