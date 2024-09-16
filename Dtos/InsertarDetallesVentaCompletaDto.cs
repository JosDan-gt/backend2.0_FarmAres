namespace GranjaLosAres_API.Dtos
{
    public class InsertarDetallesVentaCompletaDto
    {
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public string TamanoHuevo { get; set; }
        public string TipoEmpaque { get; set; } // Nueva propiedad
        public int CantidadVendida { get; set; } // Nueva propiedad
        public decimal PrecioUnitario { get; set; } // Nueva propiedad
        public int CajasVendidas { get; set; }
        public decimal PrecioPorCaja { get; set; }
        public int CartonesVendidos { get; set; }
        public decimal PrecioPorCarton { get; set; }
        public int HuevosSueltosVendidos { get; set; }
        public decimal PrecioPorHuevoSuelto { get; set; }
    }
}
