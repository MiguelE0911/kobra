using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>
/// Reglas de presentación de los reportes de Pagos: estado de cuenta del cliente y antigüedad
/// de saldos. Los importes vienen de la vista de saldos; aquí solo se agrupan en tramos.
/// </summary>
public class ReporteNegocio
{
    private static readonly string[] NombresTramos = ["Corriente", "1-30", "31-60", "61-90", "Más de 90"];

    private readonly ReporteDatos _datos;
    private readonly PagoDatos _pagoDatos;

    public ReporteNegocio() : this(new ReporteDatos(), new PagoDatos()) { }

    public ReporteNegocio(ReporteDatos datos, PagoDatos pagoDatos)
    {
        _datos = datos;
        _pagoDatos = pagoDatos;
    }

    /// <summary>Estado de cuenta de un cliente, con facturas y totales.</summary>
    public EstadoCuenta? ObtenerEstadoCuenta(int clienteId)
    {
        if (clienteId <= 0)
            throw new ReglaNegocioException("Seleccione un cliente para ver su estado de cuenta.");

        return _datos.ObtenerEstadoCuenta(clienteId);
    }

    /// <summary>Antigüedad de saldos con los mismos cinco tramos del panel de inicio.</summary>
    public AntiguedadSaldos ObtenerAntiguedad(int? clienteId)
    {
        var filtro = clienteId is > 0 ? clienteId : null;
        var facturas = _datos.ObtenerFacturasConSaldo(filtro);

        var resultado = new AntiguedadSaldos
        {
            Facturas = facturas.Count,
            SaldoTotal = facturas.Sum(f => f.Saldo)
        };

        for (var i = 0; i < NombresTramos.Length; i++)
            resultado.Tramos.Add(new TramoAntiguedad { Nombre = NombresTramos[i], Orden = i });

        foreach (var factura in facturas)
        {
            var tramo = resultado.Tramos[ObtenerOrdenTramo(factura)];
            tramo.Detalle.Add(factura);
            tramo.Facturas++;
            tramo.Saldo += factura.Saldo;
        }

        foreach (var tramo in resultado.Tramos)
            tramo.Porcentaje = resultado.SaldoTotal == 0 ? 0 : tramo.Saldo / resultado.SaldoTotal * 100m;

        return resultado;
    }

    /// <summary>Pagos de una factura, para el detalle del estado de cuenta.</summary>
    public List<Pago> ListarPagosDeFactura(int facturaId) =>
        facturaId > 0 ? _pagoDatos.ListarPorFactura(facturaId) : [];

    // privados

    private static int ObtenerOrdenTramo(LineaAntiguedad factura) => factura.DiasVencidos switch
    {
        <= 0 => 0,
        <= 30 => 1,
        <= 60 => 2,
        <= 90 => 3,
        _ => 4
    };
}
