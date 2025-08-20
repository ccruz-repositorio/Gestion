using Gestion.Application.DTOs;
using Gestion.Application.Enums;
using Gestion.Domain.Models;

namespace Gestion.Application.Interfaces.Services
{

    public interface ISalonService
    {
        Task<(ResponseCode code, string message, IEnumerable<SalonDto>? data)> GetSalonesAsync(); 
    }
}