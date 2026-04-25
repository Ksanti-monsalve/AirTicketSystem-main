// src/modules/aircraftmanufacturer/Domain/ValueObjects/NombreAircraftManufacturer.cs
namespace AirTicketSystem.modules.aircraftmanufacturer.Domain.ValueObjects;

public sealed class NombreAircraftManufacturer
{
    public string Valor { get; }

    private NombreAircraftManufacturer(string valor) => Valor = valor;

    public static NombreAircraftManufacturer Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del fabricante no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre del fabricante debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre del fabricante no puede superar 100 caracteres.");

        return new NombreAircraftManufacturer(normalizado);
    }

    public override string ToString() => Valor;
}