using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using GranjaLosAres_API.Models;
using GranjaLosAres_API.Data;


namespace GranjaLosAres_API.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly MyDbContext _context;

        public DashboardController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("infolote/{id}")]
        public async Task<ActionResult<VistaDashboard>> GetVistaDash(int id)
        {
            var resultado = await _context.VistaDashboards
                                          .Where(v => v.IdLote == id)
                                          .FirstOrDefaultAsync();

            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        public class ProduccionDto
        {
            public string FechaRegistro { get; set; }
            public int Produccion { get; set; }
            public int Defectuosos { get; set; }
        }

        [HttpGet("produccion/{idLote}/{periodo}")]
        public IActionResult GetProduccion(int idLote, string periodo)
        {
            // Consulta base para obtener todos los datos relevantes
            var baseQuery = _context.ProduccionGallinas
                .Where(p => p.IdLote == idLote && p.Estado == true)
                .OrderBy(p => p.FechaRegistroP)
                .AsEnumerable();

            var produccion = periodo switch
            {
                "diario" => baseQuery
                    .GroupBy(p => p.FechaRegistroP.Value.Date)
                    .Select(g => new ProduccionDto
                    {
                        FechaRegistro = g.Key.ToString("yyyy-MM-dd"),
                        Produccion = g.Sum(p => p.CantTotal ?? 0), // Manejar el nullable int aquí
                        Defectuosos = g.Sum(p => p.Defectuosos ?? 0) // Manejar el nullable int aquí
                    })
                    .ToList(),

                "semanal" => baseQuery
                    .GroupBy(p => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(p.FechaRegistroP.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                    .Select(g => new ProduccionDto
                    {
                        FechaRegistro = $"Semana {g.Key}",
                        Produccion = g.Sum(p => p.CantTotal ?? 0), // Manejar el nullable int aquí
                        Defectuosos = g.Sum(p => p.Defectuosos ?? 0) // Manejar el nullable int aquí
                    })
                    .ToList(),

                "mensual" => baseQuery
                    .GroupBy(p => new { p.FechaRegistroP.Value.Year, p.FechaRegistroP.Value.Month })
                    .Select(g => new ProduccionDto
                    {
                        FechaRegistro = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Produccion = g.Sum(p => p.CantTotal ?? 0), // Manejar el nullable int aquí
                        Defectuosos = g.Sum(p => p.Defectuosos ?? 0) // Manejar el nullable int aquí
                    })
                    .ToList(),

                _ => throw new ArgumentException("Período no válido")
            };

            return Ok(produccion);
        }

        [HttpGet("clasificacion/{idLote}/{periodo}")]
        public IActionResult GetClasificacion(int idLote, string periodo)
        {
            // Consulta base para obtener todos los datos relevantes en base a la fecha de producción
            var baseQuery = _context.ClasificacionHuevos
                .Include(c => c.IdProdNavigation)  // Asegurarnos de cargar la relación
                .Where(c => c.IdProdNavigation.IdLote == idLote && c.Estado == true && c.IdProdNavigation.FechaRegistroP.HasValue)
                .OrderBy(c => c.IdProdNavigation.FechaRegistroP);

            // Dependiendo del período, agrupar los datos de acuerdo con la fecha de producción
            var clasificacion = periodo switch
            {
                "diario" => baseQuery
                    .GroupBy(c => new { c.IdProdNavigation.FechaRegistroP.Value.Date, c.Tamano })
                    .Select(g => new ClasificacionDto
                    {
                        FechaRegistro = g.Key.Date.ToString("yyyy-MM-dd"),
                        Tamano = g.Key.Tamano,
                        TotalUnitaria = g.Sum(c => c.TotalUnitaria ?? 0)  // Manejar null en TotalUnitaria
                    })
                    .ToList(),

                "semanal" => baseQuery
                    .AsEnumerable() // Realizamos la agrupación en memoria después de obtener los datos de la DB
                    .GroupBy(c => new
                    {
                        Week = GetWeekOfYear(c.IdProdNavigation.FechaRegistroP.Value),
                        Year = c.IdProdNavigation.FechaRegistroP.Value.Year,
                        c.Tamano
                    })
                    .Select(g => new ClasificacionDto
                    {
                        FechaRegistro = $"Semana {g.Key.Week}",
                        Tamano = g.Key.Tamano,
                        TotalUnitaria = g.Sum(c => c.TotalUnitaria ?? 0)
                    })
                    .ToList(),


                "mensual" => baseQuery
                    .GroupBy(c => new { c.IdProdNavigation.FechaRegistroP.Value.Year, c.IdProdNavigation.FechaRegistroP.Value.Month, c.Tamano })
                    .Select(g => new ClasificacionDto
                    {
                        FechaRegistro = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Tamano = g.Key.Tamano,
                        TotalUnitaria = g.Sum(c => c.TotalUnitaria ?? 0)  // Manejar null en TotalUnitaria
                    })
                    .ToList(),

                _ => throw new ArgumentException("Período no válido")
            };

            // Devolver la clasificación o un mensaje de no encontrado
            if (clasificacion.Any())
            {
                return Ok(clasificacion);
            }
            else
            {
                return NotFound(new { message = "No se encontraron datos para el lote y período especificados." });
            }
        }

        [HttpGet("estadolote/{idLote}/{periodo}")]
        public IActionResult GetEstadoLote(int idLote, string periodo)
        {
            // Consulta base para obtener los datos del estado del lote
            var baseQuery = _context.EstadoLotes
                .Where(e => e.IdLote == idLote && e.Estado == true)
                .OrderBy(e => e.FechaRegistro)
                .AsEnumerable();

            var estadoLote = periodo switch
            {
                "diario" => baseQuery
                    .GroupBy(e => e.FechaRegistro.Date)  // Ya no necesitamos HasValue porque FechaRegistro siempre tiene un valor
                    .Select(g => new EstadoLoteDto
                    {
                        FechaRegistro = g.Key.ToString("yyyy-MM-dd"),
                        CantidadG = g.Sum(e => e.CantidadG),
                        Bajas = g.Sum(e => e.Bajas)
                    })
                    .ToList(),

                "semanal" => baseQuery
                    .GroupBy(e => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(e.FechaRegistro, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                    .Select(g => new EstadoLoteDto
                    {
                        FechaRegistro = $"Semana {g.Key}",
                        CantidadG = g.Sum(e => e.CantidadG),
                        Bajas = g.Sum(e => e.Bajas)
                    })
                    .ToList(),

                "mensual" => baseQuery
                    .GroupBy(e => new { e.FechaRegistro.Year, e.FechaRegistro.Month })
                    .Select(g => new EstadoLoteDto
                    {
                        FechaRegistro = $"{g.Key.Year}-{g.Key.Month:D2}",
                        CantidadG = g.Sum(e => e.CantidadG),
                        Bajas = g.Sum(e => e.Bajas)
                    })
                    .ToList(),

                _ => throw new ArgumentException("Período no válido")
            };

            return Ok(estadoLote);
        }





        public class ClasificacionDto
        {
            public string FechaRegistro { get; set; }
            public string Tamano { get; set; }
            public int? TotalUnitaria { get; set; }
        }
        public class EstadoLoteDto
        {
            public string FechaRegistro { get; set; }
            public int CantidadG { get; set; }
            public int Bajas { get; set; }
        }




        private int GetWeekOfYear(DateTime date)
        {
            var calendar = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            return calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

    }
}
