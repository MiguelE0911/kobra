using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>
/// Validaciones y reglas de Pago. Un pago pertenece a una sola factura y nunca puede superar
/// el saldo de esa factura, leído desde dbo.vw_FacturaSaldo.
/// </summary>
public class PagoNegocio
{
    public const int MaximoResultados = 50;

    private readonly PagoDatos _datos;
    private readonly FacturaDatos _facturaDatos;

    public PagoNegocio() : this(new PagoDatos(), new FacturaDatos()) { }

    public PagoNegocio(PagoDatos datos, FacturaDatos facturaDatos)
    {
        _datos = datos;
        _facturaDatos = facturaDatos;
    }

    /// <summary>Registra un pago y devuelve su Id. La comprobación definitiva del saldo es atómica en PagoDatos.</summary>
    public int Registrar(Pago pago)
    {
        Normalizar(pago);
        Validar(pago);

        var nuevoId = _datos.Insertar(pago);
        if (nuevoId is int id)
            return id;

        // La inserción defensiva rechazó el pago: se vuelve a leer la factura para dar un mensaje preciso.
        var factura = _facturaDatos.ObtenerPorId(pago.FacturaId);
        if (factura is null)
            throw new ReglaNegocioException("La factura seleccionada ya no existe.");

        if (factura.Anulada)
            throw Error("No se puede registrar un pago a una factura anulada.", nameof(Pago.FacturaId));

        throw Error(
            $"El monto supera el saldo disponible de la factura (B/. {factura.Saldo:N2}).",
            nameof(Pago.Monto));
    }

    /// <summary>Anulación lógica: exige motivo y comprueba que el pago siga activo.</summary>
    public void Anular(int id, string? motivo)
    {
        if (id <= 0)
            throw new ReglaNegocioException("Seleccione un pago para anular.");

        var motivoLimpio = (motivo ?? string.Empty).Trim();
        if (motivoLimpio.Length == 0)
            throw Error("El motivo de anulación es obligatorio.", nameof(Pago.MotivoAnulacion));
        if (motivoLimpio.Length > 250)
            throw Error("El motivo de anulación no puede tener más de 250 caracteres.", nameof(Pago.MotivoAnulacion));

        var actual = _datos.ObtenerPorId(id)
            ?? throw new ReglaNegocioException("El pago seleccionado ya no existe.");

        if (actual.Anulado)
            throw new ReglaNegocioException("El pago ya se encuentra anulado.");

        if (!_datos.Anular(id, motivoLimpio))
            throw new ReglaNegocioException("No se pudo anular el pago: ya estaba anulado.");
    }

    public Pago? ObtenerPorId(int id) => _datos.ObtenerPorId(id);

    public decimal ObtenerSaldoFactura(int facturaId) =>
        facturaId > 0 ? _datos.ObtenerSaldoFactura(facturaId) : 0m;

    public List<Pago> Buscar(string? texto, int? clienteId = null, bool? anulado = null) =>
        _datos.Buscar(texto, clienteId, anulado, MaximoResultados);

    public List<Pago> ListarPorFactura(int facturaId) =>
        facturaId > 0 ? _datos.ListarPorFactura(facturaId) : [];

    /// <summary>Facturas del cliente que todavía tienen saldo, ordenadas por vencimiento.</summary>
    public List<Factura> ObtenerFacturasConSaldo(int clienteId) =>
        clienteId > 0
            ? _facturaDatos.Buscar(null, clienteId, null, false, MaximoResultados)
                .Where(f => !f.Anulada && f.Saldo > 0)
                .OrderBy(f => f.FechaVencimiento)
                .ToList()
            : [];

    // privados

    private static void Normalizar(Pago p)
    {
        p.FechaPago = p.FechaPago.Date;
        p.Monto = Math.Round(p.Monto, 2, MidpointRounding.AwayFromZero);
    }

    private void Validar(Pago p)
    {
        if (p.FacturaId <= 0)
            throw Error("Seleccione una factura.", nameof(Pago.FacturaId));

        var factura = _facturaDatos.ObtenerPorId(p.FacturaId)
            ?? throw Error("La factura seleccionada ya no existe.", nameof(Pago.FacturaId));

        if (factura.Anulada)
            throw Error("No se puede pagar una factura anulada.", nameof(Pago.FacturaId));

        if (p.FechaPago.Date > DateTime.Today)
            throw Error("La fecha de pago no puede ser futura.", nameof(Pago.FechaPago));

        if (p.Monto <= 0)
            throw Error("El monto del pago debe ser mayor que 0.", nameof(Pago.Monto));

        if (p.Monto > factura.Saldo)
            throw Error(
                $"El monto supera el saldo disponible de la factura (B/. {factura.Saldo:N2}).",
                nameof(Pago.Monto));
    }

    private static ReglaNegocioException Error(string mensaje, string campo) =>
        new(mensaje) { Campo = campo };
}
