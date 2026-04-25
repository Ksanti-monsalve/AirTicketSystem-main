// src/modules/luggagerestriction/Domain/ValueObjects/LargoMaxCmLuggageRestriction.cs
namespace AirTicketSystem.modules.luggagerestriction.Domain.ValueObjects;

public sealed class LargoMaxCmLuggageRestriction
{
    public int Valor { get; }

    private LargoMaxCmLuggageRestriction(int valor) => Valor = valor;

    public static LargoMaxCmLuggageRestriction Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El largo máximo debe ser mayor a 0 cm.");

        if (valor > 300)
            throw new ArgumentException(
                "El largo máximo no puede superar 300 cm.");

        return new LargoMaxCmLuggageRestriction(valor);
    }

    public bool ExcedeLargo(int largoReal) => largoReal > Valor;

    public override string ToString() => $"{Valor} cm";
}