// src/modules/airline/Domain/ValueObjects/NombreAerolinea.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class NombreAerolinea
{
    public string Valor { get; }

    private NombreAerolinea(string valor) => Valor = valor;

    public static NombreAerolinea Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre de la aerolínea no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre de la aerolínea debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre de la aerolínea no puede superar 100 caracteres.");

        return new NombreAerolinea(normalizado);
    }

    public override string ToString() => Valor;
}