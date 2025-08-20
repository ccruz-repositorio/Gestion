using Gestion.Application.Enums;
using Gestion.Application.Interfaces.Repositories;
using Gestion.Application.Services;
using Gestion.Domain.Models;
using Moq;
using Xunit;

namespace Gestion.Tests
{
    public class SalonServiceTests
    {
        private readonly Mock<ISalonRepository> _mockSalonRepository;
        private readonly SalonService _salonService;

        public SalonServiceTests()
        {
            _mockSalonRepository = new Mock<ISalonRepository>();
            _salonService = new SalonService(_mockSalonRepository.Object);
        }

        [Fact]
        public async Task GetSalonesAsync_SalonesEncontrados_RetornaOkConDatos()
        {
            // Arrange
            var salonesExistentes = new List<Salon>
            {
                new Salon { IdSalon = 1, Nombre = "Salon Cristal" },
                new Salon { IdSalon = 2, Nombre = "Salon Esmeralda" }
            };
            _mockSalonRepository.Setup(repo => repo.GetAllAsync())
                                .ReturnsAsync(salonesExistentes);

            // Act
            var (code, message, data) = await _salonService.GetSalonesAsync();

            // Assert
            Assert.Equal(ResponseCode.Ok, code);
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task GetSalonesAsync_NoSalonesEncontrados_RetornaNotFound()
        {
            // Arrange
            _mockSalonRepository.Setup(repo => repo.GetAllAsync())
                                .ReturnsAsync(new List<Salon>());

            // Act
            var (code, message, data) = await _salonService.GetSalonesAsync();

            // Assert
            Assert.Equal(ResponseCode.NotFound, code);
            Assert.Null(data);
        }
    }
}