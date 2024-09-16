using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using GranjaLosAres_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GranjaLosAres_API.Controllers
{
    //[Authorize(Roles = "Admin, User")]
    [ApiController]
    [Route("api/lotes")]
    public class LotesController : Controller
    {
        private readonly MyDbContext _context;
        private readonly ILoteService _loteService;

        public LotesController(MyDbContext context, ILoteService loteService)
        {
            _context = context;
            _loteService = loteService;
        }

        // Obtiene los lotes, activos o dados de baja según el parámetro `dadosDeBaja`.
        //[Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lote>>> GetLotes(bool? dadosDeBaja = null)
        {
            IQueryable<Lote> query = _context.Lotes;

            if (dadosDeBaja.HasValue)
            {
                query = query.Where(l => l.EstadoBaja == dadosDeBaja.Value);
            }
            else
            {
                query = query.Where(l => l.Estado == true && l.EstadoBaja == false);
            }

            var lotes = await query.ToListAsync();

            if (lotes == null || !lotes.Any())
            {
                return NotFound(dadosDeBaja.HasValue && dadosDeBaja.Value
                    ? "No se encontraron lotes dados de baja."
                    : "No se encontraron lotes.");
            }

            return Ok(lotes);
        }

        // Registra un nuevo lote.
        //[Authorize(Roles = "Admin")]
        [HttpPost("/postLote")]
        public async Task<IActionResult> Post([FromBody] Lote lote)
        {
            if (lote == null)
            {
                return BadRequest("Lote es NULL");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _loteService.AddLoteAsync(lote);
                return Ok(new { success = true, message = "Lote registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Actualiza un lote existente.
        //[Authorize(Roles = "Admin")]
        [HttpPut("/putLote")]
        public async Task<IActionResult> Put([FromBody] Lote lote)
        {
            if (lote == null)
            {
                return BadRequest("Lote es NULL");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _loteService.UpdLoteAsync(lote);
                return Ok(new { success = true, message = "Lote actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Actualiza el estado de un lote (por ejemplo, eliminar o reactivar).
        //[Authorize(Roles = "Admin")]
        [HttpPut("/updateestadolot")]
        public async Task<IActionResult> PutEstado(int idLote, [FromBody] EstadoDto dto)
        {
            var lote = await _context.Lotes.FindAsync(idLote);
            if (lote == null)
            {
                return NotFound(new { message = "Lote no encontrado." });
            }

            lote.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Lote eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Actualiza el estado de baja de un lote.
        //[Authorize(Roles = "Admin")]
        [HttpPut("putLoteBaja")]
        public async Task<IActionResult> PutLoteBaja(int idLote, [FromBody] Lote lote)
        {
            var existingLote = await _context.Lotes.FindAsync(idLote);
            if (existingLote == null)
            {
                return NotFound("Lote no encontrado.");
            }

            existingLote.EstadoBaja = lote.EstadoBaja; // Actualiza el campo EstadoBaja
            await _context.SaveChangesAsync();

            return Ok("Estado del lote actualizado correctamente.");
        }

        // DTO para actualizar el estado del lote.
        public class EstadoDto
        {
            public bool Estado { get; set; }
        }
    }
}
