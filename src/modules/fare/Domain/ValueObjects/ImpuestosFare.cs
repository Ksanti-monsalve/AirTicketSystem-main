// src/modules/fare/Domain/ValueObjects/ImpuestosFare.cs
namespace AirTicketSystem.modules.fare.Domain.ValueObjects;

public sealed class ImpuestosFare
{
    public decimal Valor { get; }

    private ImpuestosFare(decimal valor) => Valor = valor;

    public static ImpuestosFare Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("Los impuestos no pueden ser negativos.");

        if (valor > 99_999_999)
            throw new ArgumentException(
                "Los impuestos no pueden superar 99.999.999.");

        return new ImpuestosFare(Math.Round(valor, 2));
    }

    public static ImpuestosFare Cero() => new(0);

    public override string ToString() => $"{Valor:N2}";
}