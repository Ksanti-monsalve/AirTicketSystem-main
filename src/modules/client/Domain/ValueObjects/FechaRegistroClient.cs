// src/modules/client/Domain/ValueObjects/FechaRegistroClient.cs
namespace AirTicketSystem.modules.client.Domain.ValueObjects;

public sealed class FechaRegistroClient
{
    public DateTime Valor { get; }

    private FechaRegistroClient(DateTime valor) => Valor = valor;

    public static FechaRegistroClient Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de registro del cliente no puede ser una fecha vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException("La fecha de registro no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException("La fecha de registro no puede ser anterior al año 2000.");

        return new FechaRegistroClient(valor);
    }

    public static FechaRegistroClient Ahora() => new(DateTime.UtcNow);

    // Útil para reportes y pantallas de consola
    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}