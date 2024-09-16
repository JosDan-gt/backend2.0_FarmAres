using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GranjaLosAres_API.Services
{
    public class LoteService : ILoteService
    {
        private readonly MyDbContext _context;

        public LoteService(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddLoteAsync(Lote lote)
        {
            var parameters = new[]
            {
                new SqlParameter("@newCantidad", lote.CantidadG),
                new SqlParameter("@newNumLote", lote.NumLote),
                new SqlParameter("@newIdRaza", lote.IdRaza),
                new SqlParameter("@newIdCorral", lote.IdCorral),
                new SqlParameter("@newFechaAdq", lote.FechaAdq)
    };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[InsertLote] @newCantidad, @newNumLote, @newIdRaza, @newIdCorral, @newFechaAdq", parameters);
        }


        public async Task UpdLoteAsync(Lote lote)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdLote", lote.IdLote),
                new SqlParameter("@newCantidad", lote.CantidadG),
                new SqlParameter("@newIdRaza", lote.IdRaza),
                new SqlParameter("@newIdCorral", lote.IdCorral),
                new SqlParameter("@newNumLote", lote.NumLote)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[ActualizarLote] @IdLote, @newCantidad, @newIdRaza, @newIdCorral, @newNumLote", parameters);
        }
    }
}
