// src/modules/country/Domain/ValueObjects/CodigoIso2Country.cs
namespace AirTicketSystem.modules.country.Domain.ValueObjects;

public sealed class CodigoIso2Country
{
    public string Valor { get; }

    private CodigoIso2Country(string valor) => Valor = valor;

    public static CodigoIso2Country Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código ISO2 del país no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 2)
            throw new ArgumentException(
                $"El código ISO2 debe tener exactamente 2 letras. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetter))
            throw new ArgumentException(
                $"El código ISO2 solo puede contener letras. Se recibió: '{valor}'");

        return new CodigoIso2Country(normalizado);
    }

    public override string ToString() => Valor;
}