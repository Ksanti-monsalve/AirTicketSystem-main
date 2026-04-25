// src/modules/airport/Domain/ValueObjects/DireccionAirport.cs
namespace AirTicketSystem.modules.airport.Domain.ValueObjects;

public sealed class DireccionAirport
{
    public string Valor { get; }

    private DireccionAirport(string valor) => Valor = valor;

    public static DireccionAirport Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La dirección del aeropuerto no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 5)
            throw new ArgumentException("La dirección debe tener al menos 5 caracteres.");

        if (normalizado.Length > 200)
            throw new ArgumentException("La dirección no puede superar 200 caracteres.");

        return new DireccionAirport(normalizado);
    }

    public override string ToString() => Valor;
}