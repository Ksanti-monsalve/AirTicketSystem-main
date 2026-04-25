// src/modules/fare/Domain/ValueObjects/PrecioTotalFare.cs
namespace AirTicketSystem.modules.fare.Domain.ValueObjects;

public sealed class PrecioTotalFare
{
    public decimal Valor { get; }

    private PrecioTotalFare(decimal valor) => Valor = valor;

    public static PrecioTotalFare Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("El precio total no puede ser negativo.");

        if (valor == 0)
            throw new ArgumentException("El precio total debe ser mayor a 0.");

        if (valor > 99_999_999)
            throw new ArgumentException(
                "El precio total no puede superar 99.999.999.");

        return new PrecioTotalFare(Math.Round(valor, 2));
    }

    /// <summary>
    /// Calcula y valida que el total sea coherente con base + impuestos.
    /// </summary>
    public static PrecioTotalFare Calcular(decimal precioBase, decimal impuestos)
    {
        if (precioBase < 0)
            throw new ArgumentException("El precio base no puede ser negativo.");

        if (impuestos < 0)
            throw new ArgumentException("Los impuestos no pueden ser negativos.");

        var total = precioBase + impuestos;
        return Crear(total);
    }

    public override string ToString() => $"{Valor:N2}";
}