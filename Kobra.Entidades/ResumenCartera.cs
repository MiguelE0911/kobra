namespace Kobra.Entidades;

/// <summary>Cliente con su saldo pendiente, para el panel de inicio.</summary>
public class ClienteSaldo
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? RucCedula { get; set; }
    public string? Telefono { get; set; }
    public decimal LimiteCredito { get; set; }
    public decimal Saldo { get; set; }
}

/// <summary>Totales de la cartera que muestra el panel de inicio. Todo sale de vw_FacturaSaldo.</summary>
public class ResumenCartera
{
    public decimal SaldoTotal { get; set; }
    public int FacturasConSaldo { get; set; }
    public int FacturasVencidas { get; set; }
    public int ClientesActivos { get; set; }
    public int ClientesInactivos { get; set; }

    // Saldo por tramo de antigüedad (días vencidos).
    public decimal Corriente { get; set; }
    public decimal Tramo1a30 { get; set; }
    public decimal Tramo31a60 { get; set; }
    public decimal Tramo61a90 { get; set; }
    public decimal TramoMasDe90 { get; set; }

    public List<ClienteSaldo> ClientesPrincipales { get; set; } = new();
}
