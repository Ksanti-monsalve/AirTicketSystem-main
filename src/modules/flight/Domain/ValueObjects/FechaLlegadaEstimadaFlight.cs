// src/modules/flight/Domain/ValueObjects/FechaLlegadaEstimadaFlight.cs
namespace AirTicketSystem.modules.flight.Domain.ValueObjects;

public sealed class FechaLlegadaEstimadaFlight
{
    public DateTime Valor { get; }

    private FechaLlegadaEstimadaFlight(DateTime valor) => Valor = valor;

    public static FechaLlegadaEstimadaFlight Crear(DateTime valor, DateTime fechaSalida)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de llegada estimada no puede estar vacía.");

        if (valor <= fechaSalida)
            throw new ArgumentException(
                "La fecha de llegada estimada debe ser posterior a la fecha de salida.");

        // Vuelo comercial más largo: ~20 horas
        if (valor > fechaSalida.AddHours(22))
            throw new ArgumentException(
                "La duración del vuelo no puede superar 22 horas.");

        return new FechaLlegadaEstimadaFlight(valor);
    }

    public static FechaLlegadaEstimadaFlight Reconstituir(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException(
                "La fecha de llegada estimada no puede estar vacía.");
        return new FechaLlegadaEstimadaFlight(valor);
    }

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}