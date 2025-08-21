using Gestion.Application.DTOs;
using Gestion.Application.Enums;
using Gestion.Application.Interfaces.Repositories;
using Gestion.Application.Services;
using Gestion.Application.UnitOfWork;
using Gestion.Domain.Models;
using Moq;
using Xunit;

namespace Gestion.Tests
{
    public class ReservaServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly ReservaService _reservaService;

        public ReservaServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _reservaService = new ReservaService(_unitOfWork.Object);
        }

        [Fact]
        public async Task CrearReservaAsync_ReservaExitosa_RetornaCreated()
        {
            // Arrange
            var reservaDto = new ReservaDto
            {
                IdSalon = 1,
                FechaReserva = new DateTime(2025, 8, 25),
                HoraInicio = new TimeSpan(10, 0, 0),
                HoraFin = new TimeSpan(11, 0, 0),
                NombreCliente = "Test User"
            };
            _unitOfWork.Setup(repo => repo.Reserva.GetByFechaAsync(It.IsAny<DateTime>()))
                                  .ReturnsAsync(new List<Reserva>()); // No hay reservas existentes

            // Act
            var (code, message) = await _reservaService.CrearReservaAsync(reservaDto);

            // Assert
            Assert.Equal(ResponseCode.Created, code);
        }

        [Fact]
        public async Task CrearReservaAsync_HorarioFueraDeRango_RetornaBadRequest()
        {
            // Arrange
            var reservaDto = new ReservaDto
            {
                IdSalon = 1,
                FechaReserva = new DateTime(2025, 8, 25),
                HoraInicio = new TimeSpan(8, 0, 0), // Fuera del horario (9:00 AM)
                HoraFin = new TimeSpan(10, 0, 0),
                NombreCliente = "Test User"
            };

            // Act
            var (code, message) = await _reservaService.CrearReservaAsync(reservaDto);

            // Assert
            Assert.Equal(ResponseCode.BadRequest, code);
        }

        [Fact]
        public async Task CrearReservaAsync_HoraDeFinAnteriorALaDeInicio_RetornaBadRequest()
        {
            // Arrange
            var reservaDto = new ReservaDto
            {
                IdSalon = 1,
                FechaReserva = new DateTime(2025, 8, 25),
                HoraInicio = new TimeSpan(12, 0, 0),
                HoraFin = new TimeSpan(11, 0, 0), // Hora de fin anterior
                NombreCliente = "Test User"
            };

            // Act
            var (code, message) = await _reservaService.CrearReservaAsync(reservaDto);

            // Assert
            Assert.Equal(ResponseCode.BadRequest, code);
        }

        [Fact]
        public async Task CrearReservaAsync_ReservaConSolapamientoDirecto_RetornaConflict()
        {
            // Arrange
            var reservaExistente = new Reserva
            {
                IdSalon = 1,
                FechaReserva = new DateTime(2025, 8, 25),
                HoraInicio = new TimeSpan(10, 0, 0),
                HoraFin = new TimeSpan(12, 0, 0),
                NombreCliente = "Existing User"
            };

            _unitOfWork.Setup(repo => repo.Reserva.GetByFechaAsync(It.IsAny<DateTime>()))
                                  .ReturnsAsync(new List<Reserva> { reservaExistente });

            var reservaDtoNueva = new ReservaDto
            {
                IdSalon = 1,
                FechaReserva = new DateTime(2025, 8, 25),
                HoraInicio = new TimeSpan(11, 0, 0), // Se solapa
                HoraFin = new TimeSpan(13, 0, 0),
                NombreCliente = "New User"
            };

            // Act
            var (code, message) = await _reservaService.CrearReservaAsync(reservaDtoNueva);

            // Assert
            Assert.Equal(ResponseCode.Conflict, code);
        }

        [Fact]
        public async Task GetReservasPorFechaAsync_ReservasEncontradas_RetornaOkConDatos()
        {
            // Arrange
            var reservasExistentes = new List<Reserva>
            {
                new Reserva { IdSalon = 1, FechaReserva = new DateTime(2025, 8, 25), HoraInicio = new TimeSpan(10, 0, 0), HoraFin = new TimeSpan(11, 0, 0) }
            };
            _unitOfWork.Setup(repo => repo.Reserva.GetByFechaAsync(It.IsAny<DateTime>()))
                                  .ReturnsAsync(reservasExistentes);

            // Act
            var (code, message, data) = await _reservaService.GetReservasPorFechaAsync(new DateTime(2025, 8, 25));

            // Assert
            Assert.Equal(ResponseCode.Ok, code);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetReservasPorFechaAsync_NoReservasEncontradas_RetornaNotFound()
        {
            // Arrange
            _unitOfWork.Setup(repo => repo.Reserva.GetByFechaAsync(It.IsAny<DateTime>()))
                                  .ReturnsAsync(new List<Reserva>());

            // Act
            var (code, message, data) = await _reservaService.GetReservasPorFechaAsync(new DateTime(2025, 8, 25));

            // Assert
            Assert.Equal(ResponseCode.NotFound, code);
            Assert.Null(data);
        }
    }
}