
using GranjaLosAres_API.Dtos;
using GranjaLosAres_API.Models;

public class RegistrarVentaRequestDto
{
    public Venta Venta { get; set; }
    public List<InsertarDetallesVentaCompletaDto> DetallesVenta { get; set; }
}
