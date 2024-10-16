
namespace GranjaLosAres_API
{
    public class ActualizarVentaRequestDto
    {
        public int VentaId { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaVenta { get; set; }
        public List<ActualizarDetallesVentaDto> DetallesVenta { get; set; }
    }
}
