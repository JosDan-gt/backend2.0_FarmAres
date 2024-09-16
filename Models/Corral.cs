using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class Corral
{
    public int IdCorral { get; set; }

    public string NumCorral { get; set; } = null!;

    public int Capacidad { get; set; }

    public double Alto { get; set; }

    public double Ancho { get; set; }

    public double Largo { get; set; }

    public bool Agua { get; set; }

    public int Comederos { get; set; }

    public int Bebederos { get; set; }

    public int Ponederos { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();
}
