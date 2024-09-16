using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using GranjaLosAres_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GranjaLosAres_API.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    [Route("api/produccionH")]
    public class ProduccionGController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IProduccionService _produccionService;

        public ProduccionGController(MyDbContext context, IProduccionService produccionService)
        {
            _context = context;
            _produccionService = produccionService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("/getproduccion")]
        public async Task<ActionResult<IEnumerable<ProduccionGallina>>> GetProduccion(int IdLote)
        {
            var datosP = await _context.ProduccionGallinas
                                       .Where(p => p.Estado == true && p.IdLote == IdLote)
                                       .ToListAsync();
            if (datosP == null || !datosP.Any())
            {
                return NotFound("No se encontraron producciones para el lote especificado.");
            }
            return Ok(datosP);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("/postproduccion")]
        public async Task<IActionResult> Post([FromBody] ProduccionGallina produccionGallina)
        {
            if (produccionGallina == null)
            {
                return BadRequest("La producción no puede ser nula.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _produccionService.AddProduccionAsync(produccionGallina);
                return Ok(new { success = true, message = "Producción registrada exitosamente." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/updproduccion")]
        public async Task<IActionResult> Put([FromBody] ProduccionGallina produccionGallina)
        {
            if (produccionGallina == null)
            {
                return BadRequest("La producción no puede ser nula.");
            }

            var lotexist = await _context.Lotes.AnyAsync(l => l.IdLote == produccionGallina.IdLote);
            var prodexist = await _context.ProduccionGallinas.AnyAsync(g => g.IdProd == produccionGallina.IdProd);

            if (!prodexist)
            {
                return BadRequest("El IdProd no existe.");
            }
            else if (!lotexist)
            {
                return BadRequest("El IdLote no existe.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _produccionService.UpdProduccionAsync(produccionGallina);
                return Ok(new { success = true, message = "Producción actualizada exitosamente." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }


        }

    }
}
