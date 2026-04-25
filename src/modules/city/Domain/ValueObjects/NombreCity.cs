// src/modules/city/Domain/ValueObjects/NombreCity.cs
namespace AirTicketSystem.modules.city.Domain.ValueObjects;

public sealed class NombreCity
{
    public string Valor { get; }

    private NombreCity(string valor) => Valor = valor;

    public static NombreCity Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre de la ciudad no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre de la ciudad debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre de la ciudad no puede superar 100 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre de la ciudad no puede contener números.");

        return new NombreCity(normalizado);
    }

    public override string ToString() => Valor;
}