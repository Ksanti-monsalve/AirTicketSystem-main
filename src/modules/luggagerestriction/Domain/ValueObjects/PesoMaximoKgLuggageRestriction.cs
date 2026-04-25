// src/modules/luggagerestriction/Domain/ValueObjects/PesoMaximoKgLuggageRestriction.cs
namespace AirTicketSystem.modules.luggagerestriction.Domain.ValueObjects;

public sealed class PesoMaximoKgLuggageRestriction
{
    public decimal Valor { get; }

    private PesoMaximoKgLuggageRestriction(decimal valor) => Valor = valor;

    public static PesoMaximoKgLuggageRestriction Crear(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El peso máximo debe ser mayor a 0 kg.");

        // Límite práctico para equipaje de bodega
        if (valor > 50)
            throw new ArgumentException(
                "El peso máximo no puede superar 50 kg por pieza.");

        return new PesoMaximoKgLuggageRestriction(Math.Round(valor, 2));
    }

    public bool ExcedePeso(decimal pesoReal) => pesoReal > Valor;

    public decimal ExcedenteDe(decimal pesoReal)
    {
        var excedente = pesoReal - Valor;
        return excedente > 0 ? Math.Round(excedente, 2) : 0;
    }

    public override string ToString() => $"{Valor} kg";
}