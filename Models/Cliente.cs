using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GranjaLosAres_API.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string NombreCliente { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public bool? Estado { get; set; }

    [JsonIgnore]
    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
