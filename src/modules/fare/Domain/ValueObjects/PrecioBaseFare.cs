// src/modules/fare/Domain/ValueObjects/PrecioBaseFare.cs
namespace AirTicketSystem.modules.fare.Domain.ValueObjects;

public sealed class PrecioBaseFare
{
    public decimal Valor { get; }

    private PrecioBaseFare(decimal valor) => Valor = valor;

    public static PrecioBaseFare Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("El precio base no puede ser negativo.");

        if (valor == 0)
            throw new ArgumentException("El precio base debe ser mayor a 0.");

        if (valor > 99_999_999)
            throw new ArgumentException(
                "El precio base no puede superar 99.999.999.");

        return new PrecioBaseFare(Math.Round(valor, 2));
    }

    public override string ToString() => $"{Valor:N2}";
}