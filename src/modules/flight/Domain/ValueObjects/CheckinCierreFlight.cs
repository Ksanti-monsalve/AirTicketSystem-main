// src/modules/flight/Domain/ValueObjects/CheckinCierreFlight.cs
namespace AirTicketSystem.modules.flight.Domain.ValueObjects;

public sealed class CheckinCierreFlight
{
    public DateTime Valor { get; }

    private CheckinCierreFlight(DateTime valor) => Valor = valor;

    public static CheckinCierreFlight Crear(
        DateTime valor,
        DateTime fechaSalida,
        DateTime apertura)
    {
        if (valor == default)
            throw new ArgumentException("El cierre del check-in no puede estar vacío.");

        if (valor <= apertura)
            throw new ArgumentException(
                "El cierre del check-in debe ser posterior a la apertura.");

        // El check-in cierra entre 30 y 90 minutos antes del vuelo
        if (valor >= fechaSalida)
            throw new ArgumentException(
                "El check-in debe cerrar antes de la salida del vuelo.");

        if (valor < fechaSalida.AddMinutes(-90))
            throw new ArgumentException(
                "El check-in no puede cerrar más de 90 minutos antes del vuelo.");

        return new CheckinCierreFlight(valor);
    }

    public bool EstaCerrado => DateTime.UtcNow >= Valor;

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm");
}