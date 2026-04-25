// src/modules/continent/Domain/ValueObjects/CodigoContinent.cs
namespace AirTicketSystem.modules.continent.Domain.ValueObjects;

public sealed class CodigoContinent
{
    public string Valor { get; }

    private CodigoContinent(string valor) => Valor = valor;

    public static CodigoContinent Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código del continente no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 2)
            throw new ArgumentException(
                $"El código del continente debe tener exactamente 2 letras. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetter))
            throw new ArgumentException(
                $"El código del continente solo puede contener letras. Se recibió: '{valor}'");

        return new CodigoContinent(normalizado);
    }

    public override string ToString() => Valor;
}