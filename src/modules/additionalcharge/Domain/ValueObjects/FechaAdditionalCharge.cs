// src/modules/additionalcharge/Domain/ValueObjects/FechaAdditionalCharge.cs
namespace AirTicketSystem.modules.additionalcharge.Domain.ValueObjects;

public sealed class FechaAdditionalCharge
{
    public DateTime Valor { get; }

    private FechaAdditionalCharge(DateTime valor) => Valor = valor;

    public static FechaAdditionalCharge Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException(
                "La fecha del cargo adicional no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha del cargo adicional no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha del cargo adicional no puede ser anterior al año 2000.");

        return new FechaAdditionalCharge(valor);
    }

    public static FechaAdditionalCharge Ahora() => new(DateTime.UtcNow);

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}