using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models;

public partial class VistaStockRestanteHuevo
{
    public int IdProduccion { get; set; }

    public DateTime? FechaProdu { get; set; }

    public int IdLote { get; set; }

    public int? CantidadTotalProduccion { get; set; }

    public int? StockRestante { get; set; }

    public int? CajasRestantes { get; set; }

    public int? CartonesRestantes { get; set; }

    public int? HuevosSueltosRestantes { get; set; }
}
