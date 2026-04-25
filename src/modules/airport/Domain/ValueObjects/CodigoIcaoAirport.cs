// src/modules/airport/Domain/ValueObjects/CodigoIcaoAirport.cs
namespace AirTicketSystem.modules.airport.Domain.ValueObjects;

public sealed class CodigoIcaoAirport
{
    public string Valor { get; }

    private CodigoIcaoAirport(string valor) => Valor = valor;

    public static CodigoIcaoAirport Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código ICAO del aeropuerto no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 4)
            throw new ArgumentException(
                $"El código ICAO del aeropuerto debe tener exactamente 4 letras. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetter))
            throw new ArgumentException(
                $"El código ICAO del aeropuerto solo puede contener letras. Se recibió: '{valor}'");

        return new CodigoIcaoAirport(normalizado);
    }

    public override string ToString() => Valor;
}