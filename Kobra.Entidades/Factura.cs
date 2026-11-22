namespace Kobra.Entidades;

public class Factura
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.Today;
    public DateTime FechaVencimiento { get; set; } = DateTime.Today.AddDays(30);
    public decimal Subtotal { get; set; }
    public decimal Itbms { get; set; }
    public decimal Total { get; set; }
    public bool Anulada { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime? FechaAnulacion { get; set; }
    public DateTime FechaCreacion { get; set; }

    // Datos de consulta (JOIN con Cliente y vista dbo.vw_FacturaSaldo)
    public string ClienteCodigo { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;
    public string? ClienteRucCedula { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal Saldo { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public bool EstaVencida { get; set; }
    public int DiasVencidos { get; set; }

    public string ClienteDescripcion =>
        string.IsNullOrEmpty(ClienteCodigo) ? ClienteNombre : $"{ClienteCodigo} - {ClienteNombre}";

    public string TextoVencida =>
        Anulada ? "No (Anulada)" : EstaVencida ? $"Sí ({DiasVencidos} d)" : "No";
}

/// <summary>Representación de solo lectura de los pagos asociados a una factura en consulta.</summary>
public class PagoFactura
{
    public int Id { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal Monto { get; set; }
    public bool Anulado { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime FechaCreacion { get; set; }

    public string EstadoTexto => Anulado ? "Anulado" : "Activo";
}