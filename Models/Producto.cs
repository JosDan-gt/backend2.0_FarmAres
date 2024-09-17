using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GranjaLosAres_API.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    [JsonIgnore]
    public virtual ICollection<DetallesVentum> DetallesVenta { get; set; } = new List<DetallesVentum>();
}
