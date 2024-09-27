using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using GranjaLosAres_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GranjaLosAres_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/estadolote")]
    public class EstadoLoteController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IEstadoLoteService _estadoLoteService;

        public EstadoLoteController(MyDbContext context, IEstadoLoteService estadoLoteService)
        {
            _context = context;
            _estadoLoteService = estadoLoteService;
        }

        [HttpGet("/getestadolote")]
        public async Task<ActionResult<IEnumerable<EstadoLote>>> GetEstadoLote(int idLote)
        {
            var estadoLote = await _context.EstadoLotes
                                           .Where(e => e.Estado == true && e.IdLote == idLote)
                                            .ToListAsync();

            if (estadoLote == null)
            {
                return NotFound("NO EXISTE NINGUN REGISTRO");
            }

            return Ok(estadoLote);
        }




        [HttpPost("/postestadolote")]
        public async Task<IActionResult> Post([FromBody] EstadoLote estadoLote)
        {
            if (estadoLote == null)
            {
                return BadRequest("EstadoLote es NULL");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _estadoLoteService.AddEstadoLoteAsync(estadoLote);
                return Ok(new { success = true, message = "Estado registrado exitosamente." });
            }
            catch (Exception ex)
            {
                // Log del error detallado
                Console.WriteLine($"Error al registrar el EstadoLote: {ex.Message}");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


        [HttpPut("/putestadolote")]
        public async Task<IActionResult> Put([FromBody] EstadoLote estadoLote)
        {
            if (estadoLote == null)
            {
                return BadRequest("estadolote es NULL");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _estadoLoteService.UpdEstadoLoteAsync(estadoLote);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPut("updateestado/{id}")]
        public async Task<IActionResult> PutEstado(int id, [FromBody] EstadoDtoState dto)
        {
            var estadoLote = await _context.EstadoLotes.FindAsync(id);
            if (estadoLote == null)
            {
                return NotFound(new { message = "Estado Lote no encontrado." });
            }

            // Guardamos la cantidad de bajas antes de realizar la actualización
            var bajasDelRegistroEliminado = estadoLote.Bajas;

            // Actualizamos el estado a eliminado lógicamente
            estadoLote.Estado = dto.Estado;

            // Obtener el lote relacionado
            var lote = await _context.Lotes.FindAsync(estadoLote.IdLote);
            if (lote == null)
            {
                return NotFound(new { message = "Lote no encontrado." });
            }

            // Sumar las bajas del registro eliminado a la cantidad actual de gallinas en el lote
            lote.CantidadGctual += bajasDelRegistroEliminado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Estado eliminado correctamente", bajasDelRegistroEliminado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }



        public class EstadoDtoState
        {
            public bool Estado { get; set; }
        }
    }
}
