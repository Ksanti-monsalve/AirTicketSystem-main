// src/modules/airport/Domain/ValueObjects/NombreAirport.cs
namespace AirTicketSystem.modules.airport.Domain.ValueObjects;

public sealed class NombreAirport
{
    public string Valor { get; }

    private NombreAirport(string valor) => Valor = valor;

    public static NombreAirport Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del aeropuerto no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 3)
            throw new ArgumentException("El nombre del aeropuerto debe tener al menos 3 caracteres.");

        if (normalizado.Length > 150)
            throw new ArgumentException("El nombre del aeropuerto no puede superar 150 caracteres.");

        return new NombreAirport(normalizado);
    }

    public override string ToString() => Valor;
}