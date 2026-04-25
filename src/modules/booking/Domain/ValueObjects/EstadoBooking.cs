// src/modules/booking/Domain/ValueObjects/EstadoBooking.cs
namespace AirTicketSystem.modules.booking.Domain.ValueObjects;

public sealed class EstadoBooking
{
    private static readonly string[] _estadosValidos =
    {
        "PENDIENTE",
        "CONFIRMADA",
        "CANCELADA",
        "EXPIRADA"
    };

    public string Valor { get; }

    private EstadoBooking(string valor) => Valor = valor;

    public static EstadoBooking Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado de la reserva no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoBooking(normalizado);
    }

    // Métodos de fábrica expresivos
    public static EstadoBooking Pendiente() => new("PENDIENTE");
    public static EstadoBooking Confirmada() => new("CONFIRMADA");
    public static EstadoBooking Cancelada() => new("CANCELADA");
    public static EstadoBooking Expirada() => new("EXPIRADA");

    // Propiedades de negocio
    public bool EstaActiva =>
        Valor == "PENDIENTE" || Valor == "CONFIRMADA";

    public bool PermiteEmitirTiquete =>
        Valor == "CONFIRMADA";

    public bool PermiteCancelacion =>
        Valor == "PENDIENTE" || Valor == "CONFIRMADA";

    public bool EstaFinalizada =>
        Valor == "CANCELADA" || Valor == "EXPIRADA";

    public bool RequierePago =>
        Valor == "PENDIENTE";

    // Transiciones válidas — regla de negocio central
    public bool PuedeTransicionarA(EstadoBooking nuevoEstado)
    {
        return Valor switch
        {
            "PENDIENTE"  => nuevoEstado.Valor is "CONFIRMADA" or "CANCELADA" or "EXPIRADA",
            "CONFIRMADA" => nuevoEstado.Valor is "CANCELADA",
            "CANCELADA"  => false,
            "EXPIRADA"   => false,
            _            => false
        };
    }

    public override string ToString() => Valor;
}