using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GranjaLosAres_API.Services
{
    public class ClasificacionHuevoService : IClasificacionHuevoService
    {
        private readonly MyDbContext _context;

        public ClasificacionHuevoService(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddClasificacionHuevoAsync(ClasificacionHuevo clasificacionHuevo)
        {
            var parameters = new[]
            {
                new SqlParameter("@Tamano", clasificacionHuevo.Tamano),
                new SqlParameter("@Cajas", clasificacionHuevo.Cajas),
                new SqlParameter("@CartonesExtras", clasificacionHuevo.CartonesExtras),
                new SqlParameter("@HuevosSueltos", clasificacionHuevo.HuevosSueltos),
                new SqlParameter("@IdProd", clasificacionHuevo.IdProd),
                new SqlParameter("@FechaClas", clasificacionHuevo.FechaClaS)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[InsertarClasificacionHuevos] @Tamano, @Cajas, @CartonesExtras, @HuevosSueltos, @IdProd, @FechaClas", parameters);
        }


        public async Task UpdateClasificacionHuevosAsync(ClasificacionHuevo UpClasificacionHuevo)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdClasificacion", UpClasificacionHuevo.Id),
                new SqlParameter("@Tamano", UpClasificacionHuevo.Tamano),
                new SqlParameter("@Cajas", UpClasificacionHuevo.Cajas),
                new SqlParameter("@CartonesExtras", UpClasificacionHuevo.CartonesExtras),
                new SqlParameter("@HuevosSueltos", UpClasificacionHuevo.HuevosSueltos),
                new SqlParameter("@IdProd", UpClasificacionHuevo.IdProd)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[ActualizarClasificacionHuevos] @IdClasificacion, @Tamano, @Cajas, @CartonesExtras, @HuevosSueltos, @IdProd", parameters);
        }
    }
}
