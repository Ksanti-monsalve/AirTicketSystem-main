// src/modules/airline/Domain/ValueObjects/NombreComercialAerolinea.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class NombreComercialAerolinea
{
    public string Valor { get; }

    private NombreComercialAerolinea(string valor) => Valor = valor;

    public static NombreComercialAerolinea Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre comercial no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre comercial no puede superar 100 caracteres.");

        return new NombreComercialAerolinea(normalizado);
    }

    public override string ToString() => Valor;
}