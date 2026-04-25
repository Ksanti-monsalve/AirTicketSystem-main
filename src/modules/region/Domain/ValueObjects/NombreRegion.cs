// src/modules/region/Domain/ValueObjects/NombreRegion.cs
namespace AirTicketSystem.modules.region.Domain.ValueObjects;

public sealed class NombreRegion
{
    public string Valor { get; }

    private NombreRegion(string valor) => Valor = valor;

    public static NombreRegion Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre de la región no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre de la región debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre de la región no puede superar 100 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre de la región no puede contener números.");

        return new NombreRegion(normalizado);
    }

    public override string ToString() => Valor;
}