using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GranjaLosAres_API.Models;

public partial class DetallesVentum
{
    public int DetalleId { get; set; }

    public int? VentaId { get; set; }

    public int? ProductoId { get; set; }

    public string? TipoEmpaque { get; set; }

    public string? TamanoHuevo { get; set; }

    public int? CantidadVendida { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public int? TotalHuevos { get; set; }

    public decimal? Total { get; set; }

    public bool? Estado { get; set; }

    [JsonIgnore]
    public virtual Producto? Producto { get; set; }

    public virtual Venta? Venta { get; set; }
}
