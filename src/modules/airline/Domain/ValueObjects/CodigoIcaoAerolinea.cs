// src/modules/airline/Domain/ValueObjects/CodigoIcaoAerolinea.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class CodigoIcaoAerolinea
{
    public string Valor { get; }

    private CodigoIcaoAerolinea(string valor) => Valor = valor;

    public static CodigoIcaoAerolinea Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código ICAO de aerolínea no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 3)
            throw new ArgumentException(
                $"El código ICAO de aerolínea debe tener exactamente 3 letras. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetter))
            throw new ArgumentException(
                $"El código ICAO de aerolínea solo puede contener letras. Se recibió: '{valor}'");

        return new CodigoIcaoAerolinea(normalizado);
    }

    public override string ToString() => Valor;
}