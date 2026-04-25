// src/modules/airline/Domain/ValueObjects/CodigoIataAerolinea.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class CodigoIataAerolinea
{
    public string Valor { get; }

    private CodigoIataAerolinea(string valor) => Valor = valor;

    public static CodigoIataAerolinea Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código IATA de aerolínea no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 2)
            throw new ArgumentException(
                $"El código IATA de aerolínea debe tener exactamente 2 letras. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetter))
            throw new ArgumentException(
                $"El código IATA de aerolínea solo puede contener letras. Se recibió: '{valor}'");

        return new CodigoIataAerolinea(normalizado);
    }

    public override string ToString() => Valor;
}