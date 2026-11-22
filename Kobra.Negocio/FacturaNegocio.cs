using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>
/// Validaciones y reglas de negocio de Factura.
/// </summary>
public class FacturaNegocio
{
    public const int MaximoResultados = 50;

    private readonly FacturaDatos _datos;
    private readonly ClienteDatos _clienteDatos;

    public FacturaNegocio() : this(new FacturaDatos(), new ClienteDatos()) { }

    public FacturaNegocio(FacturaDatos datos, ClienteDatos clienteDatos)
    {
        _datos = datos;
        _clienteDatos = clienteDatos;
    }

    public int Registrar(Factura factura)
    {
        Normalizar(factura);
        Validar(factura);
        return _datos.Insertar(factura);
    }

    public void Anular(int id, string? motivo)
    {
        if (id <= 0)
            throw new ReglaNegocioException("Seleccione una factura para anular.");

        var motivoLimpio = (motivo ?? string.Empty).Trim();
        if (motivoLimpio.Length == 0)
            throw Error("El motivo de anulación es obligatorio.", nameof(Factura.MotivoAnulacion));
        if (motivoLimpio.Length > 250)
            throw Error("El motivo de anulación no puede tener más de 250 caracteres.", nameof(Factura.MotivoAnulacion));

        var actual = _datos.ObtenerPorId(id)
            ?? throw new ReglaNegocioException("La factura seleccionada ya no existe.");

        if (actual.Anulada)
            throw new ReglaNegocioException("La factura ya se encuentra anulada.");

        if (_datos.TienePagosActivos(id))
            throw new ReglaNegocioException(
                "No se puede anular una factura que tiene pagos activos. Anule primero sus pagos.");

        if (!_datos.Anular(id, motivoLimpio))
            throw new ReglaNegocioException(
                "No se pudo anular la factura: ya está anulada o se le acaba de registrar un pago.");
    }

    public Factura? ObtenerPorId(int id) => _datos.ObtenerPorId(id);

    public Factura? ObtenerPorNumero(string? numero)
    {
        var limpio = (numero ?? string.Empty).Trim();
        return limpio.Length == 0 ? null : _datos.ObtenerPorNumero(limpio);
    }

    public decimal ObtenerSaldoCliente(int clienteId) =>
        clienteId > 0 ? _datos.ObtenerSaldoCliente(clienteId) : 0m;

    public List<Factura> Buscar(string? texto, int? clienteId = null, string? estado = null, bool soloVencidas = false) =>
        _datos.Buscar(texto, clienteId, estado, soloVencidas, MaximoResultados);

    public List<PagoFactura> ListarPagosDeFactura(int facturaId) =>
        facturaId > 0 ? _datos.ListarPagosDeFactura(facturaId) : [];

    // privados

    private static void Normalizar(Factura f)
    {
        f.Numero = (f.Numero ?? string.Empty).Trim().ToUpperInvariant();
        f.FechaEmision = f.FechaEmision.Date;
        f.FechaVencimiento = f.FechaVencimiento.Date;
        f.Subtotal = Math.Round(f.Subtotal, 2, MidpointRounding.AwayFromZero);
        f.Itbms = Math.Round(f.Itbms, 2, MidpointRounding.AwayFromZero);
        f.Total = Math.Round(f.Subtotal + f.Itbms, 2, MidpointRounding.AwayFromZero);
    }

    private void Validar(Factura f)
    {
        if (f.ClienteId <= 0)
            throw Error("Seleccione un cliente.", nameof(Factura.ClienteId));

        var cliente = _clienteDatos.ObtenerPorId(f.ClienteId)
            ?? throw Error("El cliente seleccionado no existe.", nameof(Factura.ClienteId));

        if (!cliente.Activo)
            throw Error("No se puede facturar a un cliente inactivo.", nameof(Factura.ClienteId));

        if (f.Numero.Length == 0)
            throw Error("El número de factura es obligatorio.", nameof(Factura.Numero));
        if (f.Numero.Length > 30)
            throw Error("El número de factura no puede tener más de 30 caracteres.", nameof(Factura.Numero));

        if (f.FechaVencimiento < f.FechaEmision)
            throw Error("La fecha de vencimiento no puede ser anterior a la emisión.", nameof(Factura.FechaVencimiento));

        if (f.Subtotal < 0)
            throw Error("El subtotal no puede ser negativo.", nameof(Factura.Subtotal));
        if (f.Itbms < 0)
            throw Error("El ITBMS no puede ser negativo.", nameof(Factura.Itbms));
        if (f.Total <= 0)
            throw Error("El total de la factura debe ser mayor que 0.", nameof(Factura.Total));

        if (_datos.ExisteNumero(f.Numero, f.Id))
            throw Error("Ya existe una factura con ese número.", nameof(Factura.Numero));

        var saldoActual = _datos.ObtenerSaldoCliente(cliente.Id);
        if (saldoActual + f.Total > cliente.LimiteCredito)
        {
            var disponible = Math.Max(0m, cliente.LimiteCredito - saldoActual);
            throw Error(
                $"La factura excede el límite de crédito del cliente (Límite: B/. {cliente.LimiteCredito:N2}, " +
                $"Saldo actual: B/. {saldoActual:N2}, Disponible: B/. {disponible:N2}).",
                nameof(Factura.Subtotal));
        }
    }

    private static ReglaNegocioException Error(string mensaje, string campo) =>
        new(mensaje) { Campo = campo };
}