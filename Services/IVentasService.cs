using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using GranjaLosAres_API.Dtos;

namespace GranjaLosAres_API.Services
{
    public class VentasService : IVentasService
    {
        private readonly MyDbContext _context;

        public VentasService(MyDbContext context)
        {
            _context = context;
        }

        public async Task InsertarProductoAsync(Producto producto)
        {
            var parameters = new[]
            {
                new SqlParameter("@NombreProducto", producto.NombreProducto),
                new SqlParameter("@Descripcion", producto.Descripcion ?? (object)DBNull.Value),
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[InsertarProducto] @NombreProducto, @Descripcion", parameters);
        }

        public async Task UpdProductoAsync(Producto producto)
        {
            var parameters = new[]
            {
                new SqlParameter("@ProductoId", producto.ProductoId),
                new SqlParameter("@NombreProducto", producto.NombreProducto),
                new SqlParameter("@Descripcion", producto.Descripcion ?? (object)DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[ActualizarProducto] @ProductoId, @NombreProducto, @Descripcion", parameters);
        }

        public async Task InsertarClienteAsync(Cliente cliente)
        {
            var parameters = new[]
            {
                new SqlParameter("@NombreCliente", cliente.NombreCliente),
                new SqlParameter("@Direccion", cliente.Direccion ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", cliente.Telefono ?? (object)DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[InsertarCliente] @NombreCliente, @Direccion, @Telefono", parameters);
        }

        public async Task UpdClienteAsync(Cliente cliente)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClienteID", cliente.ClienteId),
                new SqlParameter("@NombreCliente", cliente.NombreCliente),
                new SqlParameter("@Direccion", cliente.Direccion ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", cliente.Telefono ?? (object)DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[ActualizarCliente] @ClienteID, @NombreCliente, @Direccion, @Telefono", parameters);
        }

        public async Task InsertarVentaAsync(Venta venta) // Implementación del método faltante
        {
            var parameters = new[]
            {
                new SqlParameter("@FechaVenta", venta.FechaVenta),
                new SqlParameter("@ClienteID", venta.ClienteId),
                new SqlParameter("@TotalVenta", venta.TotalVenta ?? (object)DBNull.Value),
                new SqlParameter("@Estado", venta.Estado ?? (object)DBNull.Value),
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[InsertarVenta] @FechaVenta, @ClienteID, @TotalVenta, @Estado", parameters);
        }

        public async Task UpdVentaAsync(Venta venta) // Implementación del método faltante
        {
            var parameters = new[]
            {
                new SqlParameter("@VentaID", venta.VentaId),
                new SqlParameter("@FechaVenta", venta.FechaVenta),
                new SqlParameter("@ClienteID", venta.ClienteId),
                new SqlParameter("@TotalVenta", venta.TotalVenta ?? (object)DBNull.Value),
                new SqlParameter("@Estado", venta.Estado ?? (object)DBNull.Value),
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[ActualizarVenta] @VentaID, @FechaVenta, @ClienteID, @TotalVenta, @Estado", parameters);
        }

        public async Task RegistrarVentaAsync(Venta venta, List<InsertarDetallesVentaCompletaDto> detallesVenta)
        {
            var detallesVentaTable = new DataTable();

            // Definir las columnas que coinciden con DetallesVentaType en SQL Server
            detallesVentaTable.Columns.Add("ProductoID", typeof(int));
            detallesVentaTable.Columns.Add("TipoEmpaque", typeof(string));
            detallesVentaTable.Columns.Add("TamanoHuevo", typeof(string));
            detallesVentaTable.Columns.Add("CantidadVendida", typeof(int));
            detallesVentaTable.Columns.Add("PrecioUnitario", typeof(decimal));

            // Llenar el DataTable con los datos del DTO
            foreach (var detalle in detallesVenta)
            {
                detallesVentaTable.Rows.Add(
                    detalle.ProductoId,
                    detalle.TipoEmpaque,
                    detalle.TamanoHuevo,
                    detalle.CantidadVendida,
                    detalle.PrecioUnitario
                );
            }

            var parameters = new[]
            {
                new SqlParameter("@FechaVenta", venta.FechaVenta),
                new SqlParameter("@ClienteID", venta.ClienteId),
                new SqlParameter("@DetallesVenta", SqlDbType.Structured)
                {
                    TypeName = "dbo.DetallesVentaType",
                    Value = detallesVentaTable
                }
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[RegistrarVenta] @FechaVenta, @ClienteID, @DetallesVenta", parameters);
        }


        public async Task ActualizarVentaAsync(ActualizarVentaRequestDto venta)
        {
            var detallesVentaTable = new DataTable();

            // Definir las columnas que coinciden con DetallesVentaType en SQL Server
            detallesVentaTable.Columns.Add("ProductoID", typeof(int));
            detallesVentaTable.Columns.Add("TipoEmpaque", typeof(string));
            detallesVentaTable.Columns.Add("TamanoHuevo", typeof(string));
            detallesVentaTable.Columns.Add("CantidadVendida", typeof(int));
            detallesVentaTable.Columns.Add("PrecioUnitario", typeof(decimal));

            // Llenar el DataTable con los datos del DTO
            foreach (var detalle in venta.DetallesVenta)
            {
                detallesVentaTable.Rows.Add(
                    detalle.ProductoId,
                    detalle.TipoEmpaque,
                    detalle.TamanoHuevo,
                    detalle.CantidadVendida,
                    detalle.PrecioUnitario
                );
            }

            var parameters = new[]
            {
                new SqlParameter("@VentaID", venta.VentaId),
                new SqlParameter("@ClienteID", venta.ClienteId),
                new SqlParameter("@DetallesVenta", SqlDbType.Structured)
                {
                    TypeName = "dbo.DetallesVentaType",
                    Value = detallesVentaTable
                }
            };

            // Ejecutar el procedimiento almacenado
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[ActualizarVenta] @VentaID, @ClienteID, @DetallesVenta", parameters);
        }



    }
}
