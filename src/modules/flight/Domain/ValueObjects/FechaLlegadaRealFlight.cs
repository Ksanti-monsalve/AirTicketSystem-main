// src/modules/flight/Domain/ValueObjects/FechaLlegadaRealFlight.cs
namespace AirTicketSystem.modules.flight.Domain.ValueObjects;

public sealed class FechaLlegadaRealFlight
{
    public DateTime Valor { get; }

    private FechaLlegadaRealFlight(DateTime valor) => Valor = valor;

    public static FechaLlegadaRealFlight Crear(DateTime valor, DateTime fechaSalida)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de llegada real no puede estar vacía.");

        if (valor <= fechaSalida)
            throw new ArgumentException(
                "La fecha de llegada real debe ser posterior a la fecha de salida.");

        if (valor > fechaSalida.AddHours(30))
            throw new ArgumentException(
                "La fecha de llegada real no puede superar 30 horas desde la salida.");

        return new FechaLlegadaRealFlight(valor);
    }

    public bool LlegoATiempo(DateTime fechaLlegadaEstimada)
        => Valor <= fechaLlegadaEstimada;

    public int MinutosDeDemora(DateTime fechaLlegadaEstimada)
    {
        var diferencia = Valor - fechaLlegadaEstimada;
        return diferencia.TotalMinutes > 0
            ? (int)diferencia.TotalMinutes
            : 0;
    }

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}