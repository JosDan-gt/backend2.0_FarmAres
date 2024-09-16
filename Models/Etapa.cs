using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class Etapa
{
    public int IdEtapa { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<EstadoLote> EstadoLotes { get; set; } = new List<EstadoLote>();
}
