// src/modules/luggagerestriction/Domain/ValueObjects/AnchoMaxCmLuggageRestriction.cs
namespace AirTicketSystem.modules.luggagerestriction.Domain.ValueObjects;

public sealed class AnchoMaxCmLuggageRestriction
{
    public int Valor { get; }

    private AnchoMaxCmLuggageRestriction(int valor) => Valor = valor;

    public static AnchoMaxCmLuggageRestriction Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El ancho máximo debe ser mayor a 0 cm.");

        if (valor > 200)
            throw new ArgumentException(
                "El ancho máximo no puede superar 200 cm.");

        return new AnchoMaxCmLuggageRestriction(valor);
    }

    public bool ExcedeAncho(int anchoReal) => anchoReal > Valor;

    public override string ToString() => $"{Valor} cm";
}