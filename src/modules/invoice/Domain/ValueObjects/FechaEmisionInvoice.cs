// src/modules/invoice/Domain/ValueObjects/FechaEmisionInvoice.cs
namespace AirTicketSystem.modules.invoice.Domain.ValueObjects;

public sealed class FechaEmisionInvoice
{
    public DateTime Valor { get; }

    private FechaEmisionInvoice(DateTime valor) => Valor = valor;

    public static FechaEmisionInvoice Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de emisión de la factura no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha de emisión de la factura no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de emisión no puede ser anterior al año 2000.");

        return new FechaEmisionInvoice(valor);
    }

    public static FechaEmisionInvoice Ahora() => new(DateTime.UtcNow);

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}