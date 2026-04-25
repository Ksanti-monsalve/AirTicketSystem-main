// src/modules/luggagerestriction/Domain/ValueObjects/PiezasIncluidasLuggageRestriction.cs
namespace AirTicketSystem.modules.luggagerestriction.Domain.ValueObjects;

public sealed class PiezasIncluidasLuggageRestriction
{
    public int Valor { get; }

    private PiezasIncluidasLuggageRestriction(int valor) => Valor = valor;

    public static PiezasIncluidasLuggageRestriction Crear(int valor)
    {
        if (valor < 0)
            throw new ArgumentException(
                "El número de piezas incluidas no puede ser negativo.");

        if (valor > 10)
            throw new ArgumentException(
                "El número de piezas incluidas no puede superar 10.");

        return new PiezasIncluidasLuggageRestriction(valor);
    }

    public static PiezasIncluidasLuggageRestriction Ninguna() => new(0);

    public bool IncluyePiezas => Valor > 0;

    public override string ToString() => Valor == 0
        ? "Sin piezas incluidas"
        : $"{Valor} pieza(s) incluida(s)";
}