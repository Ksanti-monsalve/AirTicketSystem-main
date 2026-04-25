// src/modules/continent/Domain/ValueObjects/NombreContinent.cs
namespace AirTicketSystem.modules.continent.Domain.ValueObjects;

public sealed class NombreContinent
{
    public string Valor { get; }

    private NombreContinent(string valor) => Valor = valor;

    public static NombreContinent Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del continente no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre del continente debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre del continente no puede superar 100 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre del continente no puede contener números.");

        return new NombreContinent(normalizado);
    }

    public override string ToString() => Valor;
}