// src/modules/booking/Domain/ValueObjects/FechaExpiracionBooking.cs
namespace AirTicketSystem.modules.booking.Domain.ValueObjects;

public sealed class FechaExpiracionBooking
{
    public DateTime Valor { get; }

    private FechaExpiracionBooking(DateTime valor) => Valor = valor;

    public static FechaExpiracionBooking Crear(DateTime valor, DateTime fechaReserva)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de expiración no puede estar vacía.");

        if (valor <= fechaReserva)
            throw new ArgumentException(
                "La fecha de expiración debe ser posterior a la fecha de reserva.");

        // Una reserva no puede expirar a más de 72 horas de creada
        if (valor > fechaReserva.AddHours(72))
            throw new ArgumentException(
                "La fecha de expiración no puede superar 72 horas desde la reserva.");

        return new FechaExpiracionBooking(valor);
    }

    /// <summary>
    /// Crea la expiración estándar: 24 horas desde la reserva.
    /// </summary>
    public static FechaExpiracionBooking EstandarDesde(DateTime fechaReserva)
        => new(fechaReserva.AddHours(24));

    // Bypasses the 72h window rule — only for reconstituting from DB-stored values.
    public static FechaExpiracionBooking Reconstituir(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de expiración no puede estar vacía.");
        return new FechaExpiracionBooking(valor);
    }

    public bool EstaExpirada => DateTime.UtcNow > Valor;

    public int HorasRestantes
    {
        get
        {
            var diferencia = Valor - DateTime.UtcNow;
            return diferencia.TotalHours > 0 ? (int)diferencia.TotalHours : 0;
        }
    }

    public bool ExpiraProximamente
    {
        get
        {
            var diferencia = Valor - DateTime.UtcNow;
            return diferencia.TotalHours > 0 && diferencia.TotalHours <= 2;
        }
    }

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}