// src/modules/flight/Domain/ValueObjects/FechaSalidaFlight.cs
namespace AirTicketSystem.modules.flight.Domain.ValueObjects;

public sealed class FechaSalidaFlight
{
    public DateTime Valor { get; }

    private FechaSalidaFlight(DateTime valor) => Valor = valor;

    public static FechaSalidaFlight Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de salida no puede estar vacía.");

        // Un vuelo puede programarse desde hoy
        if (valor < DateTime.UtcNow.AddHours(-1))
            throw new ArgumentException(
                "La fecha de salida no puede ser en el pasado.");

        // No se programan vuelos a más de 1 año
        if (valor > DateTime.UtcNow.AddYears(1))
            throw new ArgumentException(
                "La fecha de salida no puede programarse a más de 1 año en el futuro.");

        return new FechaSalidaFlight(valor);
    }

    public static FechaSalidaFlight Reconstituir(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de salida no puede estar vacía.");
        return new FechaSalidaFlight(valor);
    }

    public bool EsHoy
    {
        get
        {
            var hoy = DateTime.UtcNow.Date;
            return Valor.Date == hoy;
        }
    }

    public bool YaSalio => Valor < DateTime.UtcNow;

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}