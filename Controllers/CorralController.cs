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
    [Route("api/corralcontroller")]
    public class CorralController : Controller
    {
        private readonly MyDbContext _context;
        private readonly ICorralService _corralservice;

        public CorralController(MyDbContext context, ICorralService corralservice)
        {
            _context = context;
            _corralservice = corralservice;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/getcorral")]
        public async Task<ActionResult<IEnumerable<Corral>>> GetCorral()
        {
            var corral = await _context.Corrals
                                       .ToListAsync();

            if (corral == null)
            {
                return NotFound("NO EXISTE NINGUN REGISTRO");
            }

            return Ok(corral);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("/postcorral")]
        public async Task<IActionResult> Post([FromBody] Corral corral)
        {

            if (corral == null)
            {
                return BadRequest("Corral es Null");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _corralservice.AddCorralAsync(corral);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("/putcorral")]
        public async Task<IActionResult> Put([FromBody] Corral corral)
        {

            if (corral == null)
            {
                return BadRequest("Corral es Null");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _corralservice.UpdCorralAsync(corral);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("/updestadocorral")]
        public async Task<IActionResult> PutCorral(int id, [FromBody] EstadoDlt dto)
        {
            var corral = await _context.Corrals.FindAsync(id);
            if (corral == null)
            {
                return NotFound(new { message = "Estado Lote no encontrada." });
            }

            corral.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Estado eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        public class EstadoDlt
        {
            public bool Estado { get; set; }
        }
    }
}
