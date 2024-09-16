using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class RazaGallina
{
    public int IdRaza { get; set; }

    public string Raza { get; set; } = null!;

    public string Origen { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string ColorH { get; set; } = null!;

    public string? CaractEspec { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();
}
