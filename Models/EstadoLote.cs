using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class EstadoLote
{
    public int IdEstado { get; set; }

    public int CantidadG { get; set; }

    public int Bajas { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int? Semana { get; set; }

    public int IdEtapa { get; set; }

    public int IdLote { get; set; }

    public bool? Estado { get; set; }

    public virtual Etapa? IdEtapaNavigation { get; set; } = null!;

    public virtual Lote? IdLoteNavigation { get; set; } = null!;
}
