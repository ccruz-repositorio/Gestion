using Gestion.Application.Enums;
using Gestion.Application.Interfaces.Services;
using Gestion.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Gestion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalonController : ControllerBase
    {
        private readonly ISalonService _salonService;

        public SalonController(ISalonService salonService)
        {
            _salonService = salonService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var (code, message, data) = await _salonService.GetSalonesAsync();

            if (code == ResponseCode.Ok)
            {
                return Ok(data);
            }

            return StatusCode((int)code, new { code, message });
        }
    }
}
