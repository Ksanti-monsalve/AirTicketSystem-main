// src/modules/country/Domain/ValueObjects/NombreCountry.cs
namespace AirTicketSystem.modules.country.Domain.ValueObjects;

public sealed class NombreCountry
{
    public string Valor { get; }

    private NombreCountry(string valor) => Valor = valor;

    public static NombreCountry Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del país no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre del país debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre del país no puede superar 100 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre del país no puede contener números.");

        return new NombreCountry(normalizado);
    }

    public override string ToString() => Valor;
}