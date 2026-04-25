// src/modules/airport/Domain/ValueObjects/CodigoIataAirport.cs
namespace AirTicketSystem.modules.airport.Domain.ValueObjects;

public sealed class CodigoIataAirport
{
    public string Valor { get; }

    private CodigoIataAirport(string valor) => Valor = valor;

    public static CodigoIataAirport Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código IATA del aeropuerto no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 3)
            throw new ArgumentException(
                $"El código IATA del aeropuerto debe tener exactamente 3 letras. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetter))
            throw new ArgumentException(
                $"El código IATA del aeropuerto solo puede contener letras. Se recibió: '{valor}'");

        return new CodigoIataAirport(normalizado);
    }

    public override string ToString() => Valor;
}