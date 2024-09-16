using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class VistaDashboard
{
    public int IdLote { get; set; }

    public int CantidadGallinas { get; set; }

    public int? CantidadGallinasActual { get; set; }

    public int Bajas { get; set; }

    public string? Raza { get; set; }

    public int ProduccionTotal { get; set; }
}
