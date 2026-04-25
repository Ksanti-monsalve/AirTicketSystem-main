// src/modules/booking/Domain/aggregate/Booking.cs
using AirTicketSystem.modules.booking.Domain.ValueObjects;

namespace AirTicketSystem.modules.booking.Domain.aggregate;

public sealed class Booking
{
    public int Id { get; private set; }
    public int ClienteId { get; private set; }
    public int VueloId { get; private set; }
    public int TarifaId { get; private set; }
    public CodigoReservaBooking CodigoReserva { get; private set; } = null!;
    public FechaReservaBooking FechaReserva { get; private set; } = null!;
    public FechaExpiracionBooking FechaExpiracion { get; private set; } = null!;
    public EstadoBooking Estado { get; private set; } = null!;
    public ValorTotalBooking ValorTotal { get; private set; } = null!;
    public ObservacionesBooking? Observaciones { get; private set; }

    private Booking() { }

    public static Booking Crear(
        int clienteId,
        int vueloId,
        int tarifaId,
        decimal valorTotal,
        string? observaciones = null)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El cliente es obligatorio.");

        if (vueloId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        if (tarifaId <= 0)
            throw new ArgumentException("La tarifa es obligatoria.");

        var ahora = DateTime.UtcNow;

        return new Booking
        {
            ClienteId      = clienteId,
            VueloId        = vueloId,
            TarifaId       = tarifaId,
            CodigoReserva  = CodigoReservaBooking.Generar(),
            FechaReserva   = FechaReservaBooking.Ahora(),
            FechaExpiracion = FechaExpiracionBooking.EstandarDesde(ahora),
            Estado         = EstadoBooking.Pendiente(),
            ValorTotal     = ValorTotalBooking.Crear(valorTotal),
            Observaciones  = observaciones is not null
                ? ObservacionesBooking.Crear(observaciones)
                : null
        };
    }

    // ── Máquina de estados ───────────────────────────────────

    public void Confirmar()
    {
        if (FechaExpiracion.EstaExpirada)
            throw new InvalidOperationException(
                "No se puede confirmar una reserva expirada. " +
                "El cliente debe crear una nueva reserva.");

        CambiarEstado(EstadoBooking.Confirmada());
    }

    public void Cancelar()
    {
        if (!Estado.PermiteCancelacion)
            throw new InvalidOperationException(
                $"No se puede cancelar una reserva en estado '{Estado}'.");

        CambiarEstado(EstadoBooking.Cancelada());
    }

    public void Expirar()
    {
        if (!FechaExpiracion.EstaExpirada)
            throw new InvalidOperationException(
                "La reserva no ha expirado aún.");

        if (Estado.EstaFinalizada)
            throw new InvalidOperationException(
                $"La reserva ya está finalizada con estado '{Estado}'.");

        CambiarEstado(EstadoBooking.Expirada());
    }

    private void CambiarEstado(EstadoBooking nuevoEstado)
    {
        if (!Estado.PuedeTransicionarA(nuevoEstado))
            throw new InvalidOperationException(
                $"No se puede cambiar el estado de '{Estado}' a '{nuevoEstado}'.");

        Estado = nuevoEstado;
    }

    // ── Gestión de reserva ───────────────────────────────────

    public void ActualizarObservaciones(string? observaciones)
    {
        if (Estado.EstaFinalizada)
            throw new InvalidOperationException(
                "No se pueden actualizar observaciones de una reserva finalizada.");

        Observaciones = observaciones is not null
            ? ObservacionesBooking.Crear(observaciones)
            : null;
    }

    public void ExtenderExpiracion(int horas)
    {
        if (horas <= 0)
            throw new ArgumentException(
                "Las horas de extensión deben ser mayores a 0.");

        if (Estado.EstaFinalizada)
            throw new InvalidOperationException(
                "No se puede extender la expiración de una reserva finalizada.");

        if (FechaExpiracion.EstaExpirada)
            throw new InvalidOperationException(
                "No se puede extender una reserva ya expirada.");

        var nuevaExpiracion = FechaExpiracion.Valor.AddHours(horas);
        FechaExpiracion = FechaExpiracionBooking.Crear(
            nuevaExpiracion, FechaReserva.Valor);
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaActiva => Estado.EstaActiva;

    public bool EstaConfirmada => Estado.Valor == "CONFIRMADA";

    public bool EstaExpirada => FechaExpiracion.EstaExpirada;

    public bool PuedeEmitirTiquetes => Estado.PermiteEmitirTiquete;

    public bool RequierePago => Estado.RequierePago;

    public bool ExpiraProximamente => FechaExpiracion.ExpiraProximamente;

    public int HorasParaExpirar => FechaExpiracion.HorasRestantes;

    public static Booking Reconstituir(
        int id,
        int clienteId,
        int vueloId,
        int tarifaId,
        string codigoReserva,
        DateTime fechaReserva,
        DateTime fechaExpiracion,
        string estado,
        decimal valorTotal,
        string? observaciones)
    {
        var booking = new Booking
        {
            ClienteId      = clienteId,
            VueloId        = vueloId,
            TarifaId       = tarifaId,
            CodigoReserva  = CodigoReservaBooking.Crear(codigoReserva),
            FechaReserva   = FechaReservaBooking.Crear(fechaReserva),
            FechaExpiracion = FechaExpiracionBooking.Reconstituir(fechaExpiracion),
            Estado         = EstadoBooking.Crear(estado),
            ValorTotal     = ValorTotalBooking.Crear(valorTotal),
            Observaciones  = observaciones is not null
                ? ObservacionesBooking.Crear(observaciones)
                : null
        };
        booking.Id = id;
        return booking;
    }

    public void EstablecerId(int id) => Id = id;

    public override string ToString() =>
        $"[{CodigoReserva}] — {Estado} | {ValorTotal}";
}