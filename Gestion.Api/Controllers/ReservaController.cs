using Gestion.Application.DTOs;
using Gestion.Application.Enums;
using Gestion.Application.Interfaces.Services;
using Gestion.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Gestion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReservaDto reservaDto)
        {
            var (code, message) = await _reservaService.CrearReservaAsync(reservaDto);

            return StatusCode((int)code, new { code, message });
        }

        [HttpGet("{fecha}")]
        public async Task<IActionResult> Get(DateTime fecha)
        {
            var (code, message, data) = await _reservaService.GetReservasPorFechaAsync(fecha);

            if (code == ResponseCode.Ok)
            {
                return Ok(data);
            }

            return StatusCode((int)code, new { code, message });
        }
    }
}
