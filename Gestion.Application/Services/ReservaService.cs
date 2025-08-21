using Gestion.Application.DTOs;
using Gestion.Application.Enums;
using Gestion.Application.Interfaces.Repositories;
using Gestion.Application.Interfaces.Services;
using Gestion.Application.UnitOfWork;
using Gestion.Domain.Models;

namespace Gestion.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int TIEMPO_ACONDICIONAMIENTO_MIN = 30;
        private static readonly TimeSpan HORA_APERTURA = new TimeSpan(9, 0, 0);
        private static readonly TimeSpan HORA_CIERRE = new TimeSpan(18, 0, 0);

        public ReservaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(ResponseCode code, string message)> CrearReservaAsync(ReservaDto nuevaReservaDto)
        {
            try
            {

                Reserva nuevaReserva = ConvertToModel(nuevaReservaDto);

                // Validaciones de negocio 
                if (nuevaReserva.HoraInicio < HORA_APERTURA || nuevaReserva.HoraFin > HORA_CIERRE)
                {
                    return (ResponseCode.BadRequest, "La reserva debe estar dentro del horario de 9:00 a 18:00 hs.");
                }

                if (nuevaReserva.HoraFin <= nuevaReserva.HoraInicio)
                {
                    return (ResponseCode.BadRequest, "La hora de finalización debe ser posterior a la hora de inicio.");
                }

                var reservasExistentes = await _unitOfWork.Reserva.GetByFechaAsync(nuevaReserva.FechaReserva);

                foreach (var reserva in reservasExistentes)
                {
                    if (nuevaReserva.IdSalon == reserva.IdSalon)
                    {
                        // 1. Validacion de franja horaria
                        var esSolapamientoTotal = nuevaReserva.HoraInicio < reserva.HoraFin && nuevaReserva.HoraFin > reserva.HoraInicio;
                        if (esSolapamientoTotal)
                        {
                            return (ResponseCode.Conflict, "El salón seleccionado ya está ocupado durante ese horario.");
                        }

                        // 2. validacion 30 min mantenimiento
                        var inicioConMargen = reserva.HoraInicio.Subtract(TimeSpan.FromMinutes(TIEMPO_ACONDICIONAMIENTO_MIN));
                        var finConMargen = reserva.HoraFin.Add(TimeSpan.FromMinutes(TIEMPO_ACONDICIONAMIENTO_MIN));

                        var esSolapamientoConMantenimiento = nuevaReserva.HoraInicio < finConMargen && nuevaReserva.HoraFin > inicioConMargen;
                        if (esSolapamientoConMantenimiento)
                        {
                            return (ResponseCode.Conflict, "No hay suficiente tiempo para el mantenimiento entre reservas. Por favor, intente con otro horario.");
                        }
                    }
                }

                await _unitOfWork.Reserva.AddAsync(nuevaReserva);
                await _unitOfWork.CompleteAsync();
                return (ResponseCode.Created, "Reserva creada con éxito.");
            }
            catch (Exception ex)
            {
                return (ResponseCode.InternalServerError, $"Ocurrió un error interno en el servidor: {ex.Message}");
            }
        }

        public async Task<(ResponseCode code, string message, IEnumerable<ReservaDto>? data)> GetReservasPorFechaAsync(DateTime fecha)
        {
            try
            {
                var reservas = await _unitOfWork.Reserva.GetByFechaAsync(fecha);
                if (reservas == null || !reservas.Any())
                {
                    return (ResponseCode.NotFound, "No hay reservas para la fecha indicada.", null);
                }

                return (ResponseCode.Ok, "Reservas encontradas con éxito.", ConvertToDTO(reservas));
            }
            catch (Exception ex)
            {
                return (ResponseCode.InternalServerError, $"Ocurrió un error interno en el servidor.{ex.Message}", null);
            }
        }

        private Reserva ConvertToModel(ReservaDto reservaDto)
        {
            // Mapeo del DTO al modelo de dominio
            var nuevaReserva = new Reserva
            {
                IdSalon = reservaDto.IdSalon,
                FechaReserva = reservaDto.FechaReserva,
                HoraInicio = reservaDto.HoraInicio,
                HoraFin = reservaDto.HoraFin,
                NombreCliente = reservaDto.NombreCliente
            };
            return nuevaReserva;
        }
        private IEnumerable<ReservaDto> ConvertToDTO(IEnumerable<Reserva> reservas)
        {
            var reservaDtos = reservas.Select(reserva => new ReservaDto
            {
                IdSalon = reserva.IdSalon,
                FechaReserva = reserva.FechaReserva,
                HoraInicio = reserva.HoraInicio,
                HoraFin = reserva.HoraFin,
                NombreCliente = reserva.NombreCliente
            });
            return reservaDtos;
        }
    }
}