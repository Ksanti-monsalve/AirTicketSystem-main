// src/modules/flight/Domain/ValueObjects/MotivoCambioEstadoFlight.cs
namespace AirTicketSystem.modules.flight.Domain.ValueObjects;

public sealed class MotivoCambioEstadoFlight
{
    public string Valor { get; }

    private MotivoCambioEstadoFlight(string valor) => Valor = valor;

    public static MotivoCambioEstadoFlight Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El motivo del cambio de estado no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 5)
            throw new ArgumentException(
                "El motivo debe tener al menos 5 caracteres para ser descriptivo.");

        if (normalizado.Length > 300)
            throw new ArgumentException(
                "El motivo no puede superar 300 caracteres.");

        return new MotivoCambioEstadoFlight(normalizado);
    }

    public override string ToString() => Valor;
}