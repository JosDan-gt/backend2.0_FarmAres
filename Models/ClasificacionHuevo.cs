using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class ClasificacionHuevo
{
    public int Id { get; set; }

    public string Tamano { get; set; } = null!;

    public int Cajas { get; set; }

    public int CartonesExtras { get; set; }

    public int HuevosSueltos { get; set; }

    public int? TotalUnitaria { get; set; }

    public int IdProd { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaClaS { get; set; }

    public virtual ProduccionGallina? IdProdNavigation { get; set; } = null!;
}
