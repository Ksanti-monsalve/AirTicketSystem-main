// src/modules/ticket/Domain/ValueObjects/FechaEmisionTicket.cs
namespace AirTicketSystem.modules.ticket.Domain.ValueObjects;

public sealed class FechaEmisionTicket
{
    public DateTime Valor { get; }

    private FechaEmisionTicket(DateTime valor) => Valor = valor;

    public static FechaEmisionTicket Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de emisión del tiquete no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha de emisión no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de emisión no puede ser anterior al año 2000.");

        return new FechaEmisionTicket(valor);
    }

    public static FechaEmisionTicket Ahora() => new(DateTime.UtcNow);

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}