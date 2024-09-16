using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MyProyect_Granja.Services
{
    public class ProduccionService : IProduccionService
    {
        private readonly MyDbContext _context;

        public ProduccionService(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddProduccionAsync(ProduccionGallina prduccionGallina)
        {
            var parameters = new[]
            {
                new SqlParameter("@CantCajas", prduccionGallina.CantCajas),
                new SqlParameter("@CantCartones", prduccionGallina.CantCartones),
                new SqlParameter("@CantSueltos", prduccionGallina.CantSueltos),
                new SqlParameter("@IdEstadoL", prduccionGallina.IdLote),
                new SqlParameter("@Defectuosos", prduccionGallina.Defectuosos),
                new SqlParameter("@Fecha", prduccionGallina.FechaRegistroP),
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[InsertarProduccionGallinas] @CantCajas, @CantCartones, @CantSueltos, @IdEstadoL, @Defectuosos, @Fecha", parameters);

        }


        public async Task UpdProduccionAsync(ProduccionGallina prduccionGallina)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdProd", prduccionGallina.IdProd),
                new SqlParameter("@CantCajas", prduccionGallina.CantCajas),
                new SqlParameter("@CantCartones", prduccionGallina.CantCartones),
                new SqlParameter("@CantSueltos", prduccionGallina.CantSueltos),
                new SqlParameter("@IdLote", prduccionGallina.IdLote),
                new SqlParameter("@Defectuosos", prduccionGallina.Defectuosos),
                new SqlParameter("@FechaRg", prduccionGallina.FechaRegistroP)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[ActualizarProduccionGallinas] @IdProd, @CantCajas, @CantCartones, @CantSueltos, @IdLote, @Defectuosos, @FechaRg", parameters);

        }
    }
}
