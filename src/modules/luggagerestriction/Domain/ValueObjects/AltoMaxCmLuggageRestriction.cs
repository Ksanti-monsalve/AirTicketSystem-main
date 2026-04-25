// src/modules/luggagerestriction/Domain/ValueObjects/AltoMaxCmLuggageRestriction.cs
namespace AirTicketSystem.modules.luggagerestriction.Domain.ValueObjects;

public sealed class AltoMaxCmLuggageRestriction
{
    public int Valor { get; }

    private AltoMaxCmLuggageRestriction(int valor) => Valor = valor;

    public static AltoMaxCmLuggageRestriction Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El alto máximo debe ser mayor a 0 cm.");

        if (valor > 200)
            throw new ArgumentException(
                "El alto máximo no puede superar 200 cm.");

        return new AltoMaxCmLuggageRestriction(valor);
    }

    public bool ExcedeAlto(int altoReal) => altoReal > Valor;

    public override string ToString() => $"{Valor} cm";
}