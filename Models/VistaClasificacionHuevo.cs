using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class VistaClasificacionHuevo
{
    public string Tamaño { get; set; } = null!;

    public int Cajas { get; set; }

    public int CartonesExtras { get; set; }

    public int HuevosSueltos { get; set; }

    public int IdProduccion { get; set; }

    public int? CantidadTotalProduccion { get; set; }

    public int? CantidadTotalClasificada { get; set; }

    public int? StockRestante { get; set; }

    public int? CajasRestantes { get; set; }

    public int? CartonesRestantes { get; set; }

    public int? HuevosSueltosRestantes { get; set; }
}
