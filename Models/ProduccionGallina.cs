using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class ProduccionGallina
{
    public int IdProd { get; set; }

    public int CantCajas { get; set; }

    public int CantCartones { get; set; }

    public int CantSueltos { get; set; }

    public int IdLote { get; set; }

    public int? Defectuosos { get; set; }

    public DateTime? FechaRegistroP { get; set; }

    public int? CantTotal { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<ClasificacionHuevo> ClasificacionHuevos { get; set; } = new List<ClasificacionHuevo>();

    public virtual Lote IdLoteNavigation { get; set; } = null!;
}
