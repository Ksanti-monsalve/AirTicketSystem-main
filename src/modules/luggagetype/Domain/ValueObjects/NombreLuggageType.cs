// src/modules/luggagetype/Domain/ValueObjects/NombreLuggageType.cs
namespace AirTicketSystem.modules.luggagetype.Domain.ValueObjects;

public sealed class NombreLuggageType
{
    public string Valor { get; }

    private NombreLuggageType(string valor) => Valor = valor;

    public static NombreLuggageType Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del tipo de equipaje no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException(
                "El nombre del tipo de equipaje debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException(
                "El nombre del tipo de equipaje no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException(
                "El nombre del tipo de equipaje no puede contener números.");

        return new NombreLuggageType(normalizado);
    }

    public override string ToString() => Valor;
}