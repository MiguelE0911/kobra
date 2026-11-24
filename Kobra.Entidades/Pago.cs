namespace Kobra.Entidades;

/// <summary>
/// Pago aplicado a una sola factura. El saldo de la factura no se guarda aquí: se calcula en
/// dbo.vw_FacturaSaldo a partir de los pagos no anulados.
/// </summary>
public class Pago
{
    public int Id { get; set; }
    public int FacturaId { get; set; }
    public DateTime FechaPago { get; set; } = DateTime.Today;
    public decimal Monto { get; set; }
    public bool Anulado { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime? FechaAnulacion { get; set; }
    public DateTime FechaCreacion { get; set; }

    // Datos de consulta (JOIN con Factura, Cliente y dbo.vw_FacturaSaldo)
    public string NumeroFactura { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public string ClienteCodigo { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;
    public decimal SaldoFactura { get; set; }
    public bool FacturaAnulada { get; set; }

    public string EstadoTexto => Anulado ? "Anulado" : "Activo";

    public string ClienteDescripcion =>
        string.IsNullOrEmpty(ClienteCodigo) ? ClienteNombre : $"{ClienteCodigo} - {ClienteNombre}";
}
