namespace Kobra.Entidades;

/// <summary>Estado de cuenta de un cliente:cabecera, facturas y totales calculados desde la vista de saldos.</summary>
public class EstadoCuenta
{
    public int ClienteId { get; set; }
    public string ClienteCodigo { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;
    public string? ClienteRucCedula { get; set; }
    public decimal LimiteCredito { get; set; }
    public bool Activo { get; set; }
    public decimal TotalFacturado { get; set; }
    public decimal TotalCobrado { get; set; }
    public decimal Saldo { get; set; }

    public List<LineaEstadoCuenta> Facturas { get; } = [];

    public string ClienteDescripcion =>
        string.IsNullOrEmpty(ClienteCodigo) ? ClienteNombre : $"{ClienteCodigo} - {ClienteNombre}";
}

/// <summary>Una factura dentro del estado de cuenta del cliente.</summary>
public class LineaEstadoCuenta
{
    public int FacturaId { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal Total { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal Saldo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool EstaVencida { get; set; }
    public int DiasVencidos { get; set; }

    public string TextoVencida =>
        EstaVencida ? $"Sí ({DiasVencidos} d)" : "No";
}

/// <summary>Reporte de antigüedad de saldos: cinco tramos y el detalle de facturas de cada uno.</summary>
public class AntiguedadSaldos
{
    public decimal SaldoTotal { get; set; }
    public int Facturas { get; set; }

    public List<TramoAntiguedad> Tramos { get; } = [];
}

/// <summary>Un tramo de antigüedad (Corriente, 1-30, 31-60, 61-90 o más de 90).</summary>
public class TramoAntiguedad
{
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public int Facturas { get; set; }
    public decimal Saldo { get; set; }
    public decimal Porcentaje { get; set; }

    public List<LineaAntiguedad> Detalle { get; } = [];
}

/// <summary>Una factura dentro del detalle de un tramo de antigüedad.</summary>
public class LineaAntiguedad
{
    public int FacturaId { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string ClienteDescripcion { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public decimal Saldo { get; set; }
    public int DiasVencidos { get; set; }
}
