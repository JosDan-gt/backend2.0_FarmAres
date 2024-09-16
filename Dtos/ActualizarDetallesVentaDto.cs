namespace GranjaLosAres_API
{
    public class ActualizarDetallesVentaDto
    {
        public int ProductoId { get; set; }
        public string TipoEmpaque { get; set; }
        public string TamanoHuevo { get; set; }
        public int CantidadVendida { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
