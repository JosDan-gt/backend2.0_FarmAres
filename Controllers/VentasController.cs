using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static GranjaLosAres_API.Controllers.LotesController;
using Microsoft.Win32;
using GranjaLosAres_API.Models;
using GranjaLosAres_API.Services;
using GranjaLosAres_API;
using GranjaLosAres_API.Data;

namespace GranjaLosAres_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentasService _ventasService;
        private readonly MyDbContext _context;

        public VentasController(IVentasService ventasService, MyDbContext context)
        {
            _ventasService = ventasService;
            _context = context;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("InsertarProducto")]
        public async Task<IActionResult> InsertarProducto([FromBody] Producto producto)
        {
            try
            {
                await _ventasService.InsertarProductoAsync(producto);
                return Ok(new { success = true, message = "Producto insertado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updProducto")]
        public async Task<IActionResult> UpdProducto([FromBody] Producto producto)
        {
            try
            {
                await _ventasService.UpdProductoAsync(producto);
                return Ok(new { success = true, message = "Producto Actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/updateestadoprod")]
        public async Task<IActionResult> PutEstado(int idProd, [FromBody] EstadoCliDto dto)
        {
            var producto = await _context.Productos.FindAsync(idProd);
            if (producto == null)
            {
                return NotFound(new { message = "Cliente no encontrado." });
            }

            producto.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Cliente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("insertCliente")]
        public async Task<IActionResult> InsertarCliente([FromBody] Cliente cliente)
        {
            try
            {
                await _ventasService.InsertarClienteAsync(cliente);
                return Ok(new { success = true, message = "Cliente insertado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updCliente")]
        public async Task<IActionResult> UpdCliente([FromBody] Cliente cliente)
        {
            try
            {
                await _ventasService.UpdClienteAsync(cliente);
                return Ok(new { success = true, message = "Cliente actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/updateestadocli")]
        public async Task<IActionResult> PutEstadoPro(int idCli, [FromBody] EstadoCliDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(idCli);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente no encontrado." });
            }

            cliente.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Cliente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        public class EstadoCliDto
        {
            public bool Estado { get; set; }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("ActualizarVenta")]
        public async Task<IActionResult> ActualizarVenta([FromBody] ActualizarVentaRequestDto actualizarVentaDto)
        {
            if (actualizarVentaDto == null || actualizarVentaDto.DetallesVenta == null || !actualizarVentaDto.DetallesVenta.Any())
            {
                return BadRequest(new { success = false, message = "Datos de venta o detalles de venta no válidos." });
            }

            try
            {
                // Llamada al servicio que maneja la lógica de actualización
                await _ventasService.ActualizarVentaAsync(actualizarVentaDto);
                return Ok(new { success = true, message = "Venta actualizada con éxito" });
            }
            catch (KeyNotFoundException ex)
            {
                // Manejo específico para cuando no se encuentra algún recurso
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejo genérico de excepciones
                return StatusCode(500, new { success = false, message = "Ocurrió un error en el servidor. " + ex.Message });
            }
        }





        [Authorize(Roles = "Admin")]
        [HttpPost("InsertarDetallesVenta")]
        public async Task<IActionResult> InsertarDetallesVenta([FromBody] RegistrarVentaRequestDto request)
        {
            try
            {
                await _ventasService.RegistrarVentaAsync(request.Venta, request.DetallesVenta);
                return Ok(new { success = true, message = "Venta registrada e insertados detalles de venta exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("ProductosActivos")]
        public async Task<IActionResult> ObtenerProductosActivos()
        {
            try
            {
                var productos = await _context.Productos
                                              .Where(p => p.Estado == true) // Validar productos activos
                                              .ToListAsync();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ClientesActivos")]
        public async Task<IActionResult> ObtenerClientesActivos()
        {
            try
            {
                var clientes = await _context.Clientes
                                             .Where(c => c.Estado == true) // Validar clientes activos
                                             .ToListAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("VentasActivas")]
        public async Task<IActionResult> ObtenerVentasActivas()
        {
            try
            {
                var ventas = await _context.Ventas
                                           .Where(v => v.Estado == true) // Validar ventas activas
                                           .Include(v => v.Cliente)
                                           .ToListAsync();
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("DetallesVentaActivos/{ventaId}")]
        public async Task<IActionResult> ObtenerDetallesVentaActivos(int ventaId)
        {
            try
            {
                var detallesVenta = await _context.DetallesVenta
                                                  .Where(dv => dv.VentaId == ventaId && dv.Estado == true) // Validar detalles activos
                                                  .Include(dv => dv.Producto)
                                                  .ToListAsync();
                return Ok(detallesVenta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("stockhuevos")]
        public async Task<IActionResult> ObtenerStock()
        {
            try
            {
                var stockh = await _context.StockHuevos.ToListAsync();
                return Ok(stockh);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

    }
}


//REGISTRAR VENTA
//{ 
//"venta": {
//    "fechaVenta": "2024-09-05T03:05:54.327Z",
//    "clienteId": 1
//  },
//  "detallesVenta": [
//    {
//      "productoId": 1,
//     "tipoEmpaque": "Cartón",
//      "tamanoHuevo": "Grande",
//      "cantidadVendida": 2,
//      "precioUnitario": 15.00
//    }
//  ]
//}
