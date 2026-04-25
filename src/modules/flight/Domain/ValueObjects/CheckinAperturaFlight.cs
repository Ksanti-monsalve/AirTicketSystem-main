// src/modules/flight/Domain/ValueObjects/CheckinAperturaFlight.cs
namespace AirTicketSystem.modules.flight.Domain.ValueObjects;

public sealed class CheckinAperturaFlight
{
    public DateTime Valor { get; }

    private CheckinAperturaFlight(DateTime valor) => Valor = valor;

    public static CheckinAperturaFlight Crear(DateTime valor, DateTime fechaSalida)
    {
        if (valor == default)
            throw new ArgumentException("La apertura del check-in no puede estar vacía.");

        // El check-in abre máximo 48 horas antes del vuelo
        if (valor < fechaSalida.AddHours(-48))
            throw new ArgumentException(
                "El check-in no puede abrirse más de 48 horas antes del vuelo.");

        // El check-in debe abrir antes de la salida
        if (valor >= fechaSalida)
            throw new ArgumentException(
                "La apertura del check-in debe ser anterior a la fecha de salida.");

        return new CheckinAperturaFlight(valor);
    }

    public bool EstaAbierto => DateTime.UtcNow >= Valor;

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm");
}