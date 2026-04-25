// src/modules/payment/Domain/aggregate/Payment.cs
using AirTicketSystem.modules.payment.Domain.ValueObjects;

namespace AirTicketSystem.modules.payment.Domain.aggregate;

public sealed class Payment
{
    public int Id { get; private set; }
    public int ReservaId { get; private set; }
    public int MetodoPagoId { get; private set; }
    public MontoPayment Monto { get; private set; } = null!;
    public EstadoPayment Estado { get; private set; } = null!;
    public ReferenciaPayment? Referencia { get; private set; }
    public FechaPagoPayment? FechaPago { get; private set; }
    public FechaVencimientoPayment FechaVencimiento { get; private set; } = null!;

    private Payment() { }

    public static Payment Crear(
        int reservaId,
        int metodoPagoId,
        decimal monto)
    {
        if (reservaId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        if (metodoPagoId <= 0)
            throw new ArgumentException("El método de pago es obligatorio.");

        var ahora = DateTime.UtcNow;

        return new Payment
        {
            ReservaId      = reservaId,
            MetodoPagoId   = metodoPagoId,
            Monto          = MontoPayment.Crear(monto),
            Estado         = EstadoPayment.Pendiente(),
            Referencia     = null,
            FechaPago      = null,
            FechaVencimiento = FechaVencimientoPayment.EstandarDesde(ahora)
        };
    }

    // ── Máquina de estados ───────────────────────────────────

    public void Aprobar(string referencia)
    {
        if (!Estado.PuedeTransicionarA(EstadoPayment.Aprobado()))
            throw new InvalidOperationException(
                $"No se puede aprobar un pago en estado '{Estado}'.");

        if (FechaVencimiento.EstaVencido)
            throw new InvalidOperationException(
                "No se puede aprobar un pago vencido. " +
                "El cliente debe iniciar un nuevo proceso de pago.");

        Referencia = ReferenciaPayment.Crear(referencia);
        FechaPago  = FechaPagoPayment.Ahora();
        Estado     = EstadoPayment.Aprobado();
    }

    public void Rechazar()
    {
        if (!Estado.PuedeTransicionarA(EstadoPayment.Rechazado()))
            throw new InvalidOperationException(
                $"No se puede rechazar un pago en estado '{Estado}'.");

        Estado = EstadoPayment.Rechazado();
    }

    public void Reintentar(int nuevoMetodoPagoId, decimal? nuevoMonto = null)
    {
        if (!Estado.PermiteReintento)
            throw new InvalidOperationException(
                $"Solo se puede reintentar un pago rechazado. " +
                $"Estado actual: '{Estado}'.");

        if (nuevoMetodoPagoId <= 0)
            throw new ArgumentException(
                "El nuevo método de pago no es válido.");

        MetodoPagoId = nuevoMetodoPagoId;

        if (nuevoMonto.HasValue)
            Monto = MontoPayment.Crear(nuevoMonto.Value);

        // Generar nuevo vencimiento desde ahora
        var ahora = DateTime.UtcNow;
        FechaVencimiento = FechaVencimientoPayment.EstandarDesde(ahora);
        Referencia       = null;
        Estado           = EstadoPayment.Pendiente();
    }

    public void Reembolsar()
    {
        if (!Estado.PuedeReembolsarse)
            throw new InvalidOperationException(
                $"No se puede reembolsar un pago en estado '{Estado}'. " +
                "Solo se pueden reembolsar pagos aprobados.");

        Estado = EstadoPayment.Reembolsado();
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaAprobado => Estado.EstaAprobado;

    public bool EstaPendiente => Estado.EstaPendiente;

    public bool EstaVencido => FechaVencimiento.EstaVencido;

    public bool VenceProximamente => FechaVencimiento.VenceProximamente;

    public bool ConfirmaReserva => Estado.ConfirmaReserva;

    public bool TieneReferencia => Referencia is not null;

    public int HorasParaVencer => FechaVencimiento.HorasRestantes;

    /// <summary>
    /// Verifica si el monto del pago cubre el valor
    /// requerido por la reserva.
    /// </summary>
    public bool CubreValorRequerido(decimal valorRequerido)
        => Monto.CubreValor(valorRequerido);

    /// <summary>
    /// Calcula el cambio si el monto supera el valor requerido.
    /// </summary>
    public decimal CalcularCambio(decimal valorRequerido)
        => Monto.CambioContra(valorRequerido);

    public static Payment Reconstituir(
        int id,
        int reservaId,
        int metodoPagoId,
        decimal monto,
        string estado,
        string? referencia,
        DateTime? fechaPago,
        DateTime? fechaVencimiento)
    {
        var payment = new Payment
        {
            ReservaId    = reservaId,
            MetodoPagoId = metodoPagoId,
            Monto        = MontoPayment.Crear(monto),
            Estado       = EstadoPayment.Crear(estado),
            Referencia   = referencia is not null
                ? ReferenciaPayment.Crear(referencia)
                : null,
            FechaPago    = fechaPago.HasValue
                ? FechaPagoPayment.Crear(fechaPago.Value)
                : null,
            FechaVencimiento = fechaVencimiento.HasValue
                ? FechaVencimientoPayment.Reconstituir(fechaVencimiento.Value)
                : FechaVencimientoPayment.EstandarDesde(DateTime.UtcNow)
        };
        payment.Id = id;
        return payment;
    }

    public void EstablecerId(int id) => Id = id;

    public override string ToString() =>
        $"Pago Reserva #{ReservaId} — {Monto} | " +
        $"{Estado} | Ref: {Referencia?.Valor ?? "Sin referencia"}";
}