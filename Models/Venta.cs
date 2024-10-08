﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GranjaLosAres_API.Models;

public partial class Venta
{
    public int VentaId { get; set; }

    public DateTime FechaVenta { get; set; }

    public int? ClienteId { get; set; }

    public bool? Estado { get; set; }

    public decimal? TotalVenta { get; set; }

    public virtual Cliente? Cliente { get; set; }

    [JsonIgnore]
    public virtual ICollection<DetallesVentum> DetallesVenta { get; set; } = new List<DetallesVentum>();
}
