using Gestion.Application.DTOs;
using Gestion.Application.Enums;
using Gestion.Application.Interfaces.Repositories;
using Gestion.Application.Interfaces.Services;
using Gestion.Application.UnitOfWork;
using Gestion.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Application.Services
{
    public class SalonService : ISalonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(ResponseCode code, string message, IEnumerable<SalonDto>? data)> GetSalonesAsync()
        {
            try
            {
                var salones = await _unitOfWork.Salon.GetAllAsync();
                if (salones == null || !salones.Any())
                {
                    return (ResponseCode.NotFound, "No tiene configurados salones aun.", null);
                }

                return (ResponseCode.Ok, "Lista de Salones.", ConvertToDTO(salones));
            }
            catch (Exception ex)
            {
                return (ResponseCode.InternalServerError, $"Ocurrió un error interno en el servidor. {ex.Message}", null);
            }
        }

        private IEnumerable<SalonDto> ConvertToDTO(IEnumerable<Salon> salones)
        {
            var salonDtos = salones.Select(salon => new SalonDto
            {
                IdSalon = salon.IdSalon,
                Nombre = salon.Nombre
            });
            return salonDtos;
        }
    }
}
