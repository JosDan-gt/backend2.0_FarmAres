using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MyProyect_Granja.Services
{
    public class CorralService : ICorralService
    {
        private readonly MyDbContext _context;
        public CorralService(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddCorralAsync(Corral corral)
        {
            var parameters = new[]
            {
                new SqlParameter("@numCorral", corral.NumCorral),
                new SqlParameter("@newCapacidad", corral.Capacidad),
                new SqlParameter("@newAlto", corral.Alto),
                new SqlParameter("@newAncho", corral.Ancho),
                new SqlParameter("@newLargo", corral.Largo),
                new SqlParameter("@newAgua", corral.Agua),
                new SqlParameter("@newComederos", corral.Comederos),
                new SqlParameter("@newBebederos", corral.Bebederos),
                new SqlParameter("@newPonederos", corral.Ponederos),
                new SqlParameter("@newEstado", corral.Estado)

            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[InsertCorral] @numCorral, @newCapacidad, @newAlto, @newAncho, @newLargo, @newAgua, @newComederos, @newBebederos, @newPonederos, @newEstado", parameters);
        }


        public async Task UpdCorralAsync(Corral corral)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdCorral", corral.IdCorral),
                new SqlParameter("@numCorral", corral.NumCorral),
                new SqlParameter("@newCapacidad", corral.Capacidad),
                new SqlParameter("@newAlto", corral.Alto),
                new SqlParameter("@newAncho", corral.Ancho),
                new SqlParameter("@newLargo", corral.Largo),
                new SqlParameter("@newAgua", corral.Agua),
                new SqlParameter("@newComederos", corral.Comederos),
                new SqlParameter("@newBebederos", corral.Bebederos),
                new SqlParameter("@newPonederos", corral.Ponederos),
                new SqlParameter("@newEstado", corral.Estado)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[ActualizarCorral] @IdCorral, @numCorral, @newCapacidad, @newAlto, @newAncho, @newLargo, @newAgua, @newComederos, @newBebederos, @newPonederos, @newEstado", parameters);
        }
    }
}