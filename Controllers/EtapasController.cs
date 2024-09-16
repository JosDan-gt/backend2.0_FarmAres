using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GranjaLosAres_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/etapas")]
    public class EtapasController : Controller
    {
        private readonly MyDbContext _context;
        public EtapasController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet("/getetapas")]
        public async Task<ActionResult<IEnumerable<Etapa>>> GetLotes()
        {
            var etapa = await _context.Etapas.ToListAsync();

            if (etapa == null || !etapa.Any())
            {
                return NotFound("No se encontraron etapas.");
            }

            return Ok(etapa);
        }
    }
}
