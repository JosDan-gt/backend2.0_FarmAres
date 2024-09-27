using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace GranjaLosAres_API.Services
{
    public class EstadoLoteService : IEstadoLoteService
    {
        private readonly MyDbContext _context;

        public EstadoLoteService(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddEstadoLoteAsync(EstadoLote estadoLote)
        {
            var parameters = new[]
            {
                new SqlParameter("@Bajas", estadoLote.Bajas),
                new SqlParameter("@Fecha_Registro", estadoLote.FechaRegistro),
                new SqlParameter("@Semana", estadoLote.Semana),
                new SqlParameter("@Id_Etapa", estadoLote.IdEtapa),
                new SqlParameter("@IdLote", estadoLote.IdLote),
                new SqlParameter("@Descripcion", estadoLote.Descripcion)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[InsertarEstadoLote] @Bajas, @Fecha_Registro, @Semana, @Id_Etapa, @IdLote, @Descripcion", parameters);
        }

        public async Task UpdEstadoLoteAsync(EstadoLote estadoLote)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id_Estado", estadoLote.IdEstado),
                new SqlParameter("@Bajas", estadoLote.Bajas),
                new SqlParameter("@Semana", estadoLote.Semana),
                new SqlParameter("@Id_Etapa", estadoLote.IdEtapa),
                new SqlParameter("@IdLote", estadoLote.IdLote),
                new SqlParameter("@Descripcion", estadoLote.Descripcion)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[ActualizarEstadoLoteRegistro] @Id_Estado, @Bajas, @Semana, @Id_Etapa, @IdLote, @Descripcion", parameters);
        }


    }
}

