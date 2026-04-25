// src/modules/bookinghistory/Domain/aggregate/BookingHistory.cs
using AirTicketSystem.modules.bookinghistory.Domain.ValueObjects;

namespace AirTicketSystem.modules.bookinghistory.Domain.aggregate;

public sealed class BookingHistory
{
    public int Id { get; private set; }
    public int ReservaId { get; private set; }
    public int? UsuarioId { get; private set; }
    public EstadoAnteriorBookingHistory EstadoAnterior { get; private set; } = null!;
    public EstadoNuevoBookingHistory EstadoNuevo { get; private set; } = null!;
    public FechaCambioBookingHistory FechaCambio { get; private set; } = null!;
    public MotivoBookingHistory? Motivo { get; private set; }

    private BookingHistory() { }

    public static BookingHistory Crear(
        int reservaId,
        string estadoAnterior,
        string estadoNuevo,
        int? usuarioId = null,
        string? motivo = null)
    {
        if (reservaId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        if (estadoAnterior == estadoNuevo)
            throw new InvalidOperationException(
                "El estado anterior y el nuevo no pueden ser iguales.");

        return new BookingHistory
        {
            ReservaId     = reservaId,
            UsuarioId     = usuarioId,
            EstadoAnterior = EstadoAnteriorBookingHistory.Crear(estadoAnterior),
            EstadoNuevo   = EstadoNuevoBookingHistory.Crear(estadoNuevo),
            FechaCambio   = FechaCambioBookingHistory.Ahora(),
            Motivo        = motivo is not null
                ? MotivoBookingHistory.Crear(motivo)
                : null
        };
    }

    // Métodos de fábrica expresivos por tipo de cambio
    public static BookingHistory CrearConfirmacion(
        int reservaId,
        int? usuarioId = null)
        => Crear(reservaId, "PENDIENTE", "CONFIRMADA", usuarioId);

    public static BookingHistory CrearCancelacion(
        int reservaId,
        string motivo,
        int? usuarioId = null)
        => Crear(reservaId, "CONFIRMADA", "CANCELADA", usuarioId, motivo);

    public static BookingHistory CrearExpiracion(int reservaId)
        => Crear(reservaId, "PENDIENTE", "EXPIRADA");

    // ── Propiedades de negocio ───────────────────────────────

    public bool FueConfirmacion =>
        EstadoNuevo.Valor == "CONFIRMADA";

    public bool FueCancelacion =>
        EstadoNuevo.Valor == "CANCELADA";

    public bool FueExpiracion =>
        EstadoNuevo.Valor == "EXPIRADA";

    public bool TieneMotivo => Motivo is not null;

    public bool FueHechoPorSistema => UsuarioId is null;

    public static BookingHistory Reconstituir(
        int id,
        int reservaId,
        string estadoAnterior,
        string estadoNuevo,
        DateTime fechaCambio,
        int? usuarioId,
        string? motivo)
    {
        var history = new BookingHistory
        {
            ReservaId      = reservaId,
            UsuarioId      = usuarioId,
            EstadoAnterior = EstadoAnteriorBookingHistory.Crear(estadoAnterior),
            EstadoNuevo    = EstadoNuevoBookingHistory.Crear(estadoNuevo),
            FechaCambio    = FechaCambioBookingHistory.Crear(fechaCambio),
            Motivo         = motivo is not null
                ? MotivoBookingHistory.Crear(motivo)
                : null
        };
        history.Id = id;
        return history;
    }

    public void EstablecerId(int id) => Id = id;

    public override string ToString() =>
        $"Reserva #{ReservaId} | {EstadoAnterior} → {EstadoNuevo} " +
        $"[{FechaCambio}]";
}