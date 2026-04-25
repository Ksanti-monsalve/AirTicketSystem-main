// src/modules/invoice/Domain/aggregate/Invoice.cs
using AirTicketSystem.modules.invoice.Domain.ValueObjects;

namespace AirTicketSystem.modules.invoice.Domain.aggregate;

public sealed class Invoice
{
    public int Id { get; private set; }
    public int ReservaId { get; private set; }
    public int DireccionFacturacionId { get; private set; }
    public NumeroFacturaInvoice NumeroFactura { get; private set; } = null!;
    public FechaEmisionInvoice FechaEmision { get; private set; } = null!;
    public SubtotalInvoice Subtotal { get; private set; } = null!;
    public ImpuestosInvoice Impuestos { get; private set; } = null!;
    public TotalInvoice Total { get; private set; } = null!;

    private Invoice() { }

    public static Invoice Crear(
        int reservaId,
        int direccionFacturacionId,
        decimal subtotal,
        decimal porcentajeImpuesto = 0)
    {
        if (reservaId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        if (direccionFacturacionId <= 0)
            throw new ArgumentException(
                "La dirección de facturación es obligatoria.");

        if (porcentajeImpuesto < 0 || porcentajeImpuesto > 100)
            throw new ArgumentException(
                "El porcentaje de impuesto debe estar entre 0 y 100.");

        var sub      = SubtotalInvoice.Crear(subtotal);
        var imp      = ImpuestosInvoice.CalcularDesde(subtotal, porcentajeImpuesto);
        var total    = TotalInvoice.Calcular(subtotal, imp.Valor);

        return new Invoice
        {
            ReservaId              = reservaId,
            DireccionFacturacionId = direccionFacturacionId,
            NumeroFactura          = NumeroFacturaInvoice.Generar(),
            FechaEmision           = FechaEmisionInvoice.Ahora(),
            Subtotal               = sub,
            Impuestos              = imp,
            Total                  = total
        };
    }

    // ── Gestión de dirección ─────────────────────────────────

    public void ActualizarDireccionFacturacion(int direccionFacturacionId)
    {
        if (direccionFacturacionId <= 0)
            throw new ArgumentException(
                "La dirección de facturación no es válida.");

        if (direccionFacturacionId == DireccionFacturacionId)
            throw new InvalidOperationException(
                "La nueva dirección de facturación es la misma que la actual.");

        DireccionFacturacionId = direccionFacturacionId;
    }

    // ── Validaciones de consistencia ─────────────────────────

    /// <summary>
    /// Verifica que el total sea coherente con subtotal + impuestos.
    /// Útil para auditorías y verificaciones de integridad.
    /// </summary>
    public bool EsCoherente =>
        Total.EsCoherenteCon(Subtotal.Valor, Impuestos.Valor);

    /// <summary>
    /// Verifica que el total de la factura cubra el monto
    /// de un pago específico.
    /// </summary>
    public bool CubrePago(decimal montoPago)
        => Total.Valor >= montoPago;

    // ── Propiedades de negocio ───────────────────────────────

    public bool TieneImpuestos => Impuestos.Valor > 0;

    public decimal PorcentajeImpuestoAplicado
    {
        get
        {
            if (Subtotal.Valor == 0) return 0;
            return Math.Round((Impuestos.Valor / Subtotal.Valor) * 100, 2);
        }
    }

    /// <summary>
    /// Genera el resumen completo de la factura para
    /// mostrar en consola al cliente.
    /// </summary>
    public string ResumenFactura
    {
        get
        {
            var lineas = new List<string>
            {
                $"╔══════════════════════════════════╗",
                $"║         FACTURA ELECTRÓNICA      ║",
                $"╠══════════════════════════════════╣",
                $"║ N°     : {NumeroFactura,-23}║",
                $"║ Fecha  : {FechaEmision.EnFormatoCorto,-23}║",
                $"║ Reserva: #{ReservaId,-22}║",
                $"╠══════════════════════════════════╣",
                $"║ Subtotal  : {Subtotal.Valor,19:N2} ║",
                $"║ Impuestos : {Impuestos.Valor,19:N2} ║",
                $"║ TOTAL     : {Total.Valor,19:N2} ║",
                $"╚══════════════════════════════════╝"
            };

            return string.Join(Environment.NewLine, lineas);
        }
    }

    public static Invoice Reconstituir(
        int id,
        int reservaId,
        int direccionFacturacionId,
        string numeroFactura,
        DateTime fechaEmision,
        decimal subtotal,
        decimal impuestos,
        decimal total)
    {
        var invoice = new Invoice
        {
            ReservaId              = reservaId,
            DireccionFacturacionId = direccionFacturacionId,
            NumeroFactura          = NumeroFacturaInvoice.Crear(numeroFactura),
            FechaEmision           = FechaEmisionInvoice.Crear(fechaEmision),
            Subtotal               = SubtotalInvoice.Crear(subtotal),
            Impuestos              = ImpuestosInvoice.Crear(impuestos),
            Total                  = TotalInvoice.Crear(total)
        };
        invoice.Id = id;
        return invoice;
    }

    public void EstablecerId(int id) => Id = id;

    public override string ToString() =>
        $"Factura [{NumeroFactura}] — " +
        $"Reserva #{ReservaId} | Total: {Total}";
}