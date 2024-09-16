using GranjaLosAres_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GranjaLosAres_API.Services;
using GranjaLosAres_API.Data;
namespace GranjaLosAres_API.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/clasificacionhuevo")]
    public class ClasificacionHController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IClasificacionHuevoService _clasificacionHuevoService;

        public ClasificacionHController(MyDbContext context, IClasificacionHuevoService clasificacionHuevoService)
        {
            _context = context;
            _clasificacionHuevoService = clasificacionHuevoService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("/clasific1")]
        public async Task<ActionResult<IEnumerable<ClasificacionHuevo>>> GetClasificacionH(int idLote)
        {
            try
            {
                // Obtener todas las producciones asociadas al lote, incluyendo la fecha de producción
                var producciones = await _context.ProduccionGallinas
                                                 .Where(p => p.IdLote == idLote)
                                                 .Select(p => new { p.IdProd, p.FechaRegistroP })
                                                 .ToListAsync();

                if (!producciones.Any())
                {
                    return NotFound("No se encontró producción asociada con el lote especificado.");
                }

                // Obtener las clasificaciones de huevos activas asociadas a los idProd
                var clasificaciones = await _context.ClasificacionHuevos
                                                    .Where(c => c.Estado == true && producciones.Select(p => p.IdProd).Contains(c.IdProd))
                                                    .ToListAsync();

                // Unir clasificaciones con producciones para obtener la fecha de producción
                var datosCH = clasificaciones.Join(producciones,
                                                    c => c.IdProd,
                                                    p => p.IdProd,
                                                    (c, p) => new { Clasificacion = c, FechaRegistroP = p.FechaRegistroP, FechaClaS = c.FechaClaS })
                                              .OrderBy(cp => cp.FechaClaS) // Ordenar por FechaClaS
                                              .Select(cp => new
                                              {
                                                  cp.Clasificacion.Id,
                                                  cp.Clasificacion.Tamano,
                                                  cp.Clasificacion.Cajas,
                                                  cp.Clasificacion.CartonesExtras,
                                                  cp.Clasificacion.HuevosSueltos,
                                                  cp.Clasificacion.TotalUnitaria,
                                                  cp.Clasificacion.IdProd,
                                                  cp.Clasificacion.Estado,
                                                  FechaClaS = cp.FechaClaS,
                                                  FechaRegistroP = cp.FechaRegistroP // Incluye la fecha de producción en la respuesta
                                              })
                                              .ToList();

                if (!datosCH.Any())
                {
                    return NotFound("No se encontraron clasificaciones de huevos para la producción especificada.");
                }

                return Ok(datosCH);
            }
            catch (Exception ex)
            {
                // Log del error
                // _logger.LogError(ex, "Error al obtener clasificaciones de huevos");

                return StatusCode(500, "Ocurrió un error al procesar la solicitud. Inténtelo de nuevo más tarde.");
            }
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet("/viewstock")]
        public async Task<ActionResult<IEnumerable<VistaStockRestanteHuevo>>> GetVistaStockRestante(int idLote)
        {
            var datosVSH = await _context.VistaStockRestanteHuevos
                                         .Where(v => v.IdLote == idLote)
                                         .ToListAsync();
            return Ok(datosVSH);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("/fechasproduccion/{idLote}")]
        public async Task<ActionResult<IEnumerable<ProduccionDto>>> GetFechasProduccion(int idLote)
        {
            try
            {
                var fechasProduccion = await _context.ProduccionGallinas
                    .Where(p => p.IdLote == idLote)
                    .Select(p => new ProduccionDto
                    {
                        FechaRegistroP = p.FechaRegistroP.Value.Date,
                        IdProd = p.IdProd
                    })
                    .Distinct()
                    .ToListAsync();

                if (!fechasProduccion.Any())
                {
                    return NotFound("No se encontraron fechas de producción para el lote especificado.");
                }

                return Ok(fechasProduccion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al procesar la solicitud. Inténtelo de nuevo más tarde.");
            }
        }

        public class ProduccionDto
        {
            public DateTime FechaRegistroP { get; set; }
            public int IdProd { get; set; }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("/postclasificacion")]
        public async Task<IActionResult> Post([FromBody] ClasificacionHuevo clasificacionHuevo)
        {
            if (clasificacionHuevo == null)
            {
                return BadRequest("ClasificacionHuevo es Null");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _clasificacionHuevoService.AddClasificacionHuevoAsync(clasificacionHuevo);
                return Ok(new { success = true, message = "Clasificacion registrada exitosamente." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }



        //[Authorize(Roles = "Admin")]
        [HttpPut("/putclasificacion")]
        public async Task<IActionResult> Put([FromBody] ClasificacionHuevo UpClasificacionHuevo)
        {
            if (UpClasificacionHuevo == null)
            {
                return BadRequest("ClasificacionHuevo es Null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _clasificacionHuevoService.UpdateClasificacionHuevosAsync(UpClasificacionHuevo);
                return Ok(new { success = true, message = "Clasificacion actualizada exitosamente." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }

        }
    }
}