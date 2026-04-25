// src/modules/additionalcharge/Domain/ValueObjects/MontoAdditionalCharge.cs
namespace AirTicketSystem.modules.additionalcharge.Domain.ValueObjects;

public sealed class MontoAdditionalCharge
{
    public decimal Valor { get; }

    private MontoAdditionalCharge(decimal valor) => Valor = valor;

    public static MontoAdditionalCharge Crear(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El monto del cargo adicional debe ser mayor a 0.");

        if (valor > 99_999_999)
            throw new ArgumentException(
                "El monto del cargo adicional no puede superar 99.999.999.");

        return new MontoAdditionalCharge(Math.Round(valor, 2));
    }

    public override string ToString() => $"{Valor:N2}";
}