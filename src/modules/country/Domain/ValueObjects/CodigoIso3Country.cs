// src/modules/country/Domain/ValueObjects/CodigoIso3Country.cs
namespace AirTicketSystem.modules.country.Domain.ValueObjects;

public sealed class CodigoIso3Country
{
    public string Valor { get; }

    private CodigoIso3Country(string valor) => Valor = valor;

    public static CodigoIso3Country Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código ISO3 del país no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 3)
            throw new ArgumentException(
                $"El código ISO3 debe tener exactamente 3 letras. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetter))
            throw new ArgumentException(
                $"El código ISO3 solo puede contener letras. Se recibió: '{valor}'");

        return new CodigoIso3Country(normalizado);
    }

    public override string ToString() => Valor;
}