// src/modules/aircraftmanufacturer/Domain/ValueObjects/SitioWebAircraftManufacturer.cs
namespace AirTicketSystem.modules.aircraftmanufacturer.Domain.ValueObjects;

public sealed class SitioWebAircraftManufacturer
{
    public string Valor { get; }

    private SitioWebAircraftManufacturer(string valor) => Valor = valor;

    public static SitioWebAircraftManufacturer Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El sitio web del fabricante no puede estar vacío.");

        var normalizado = valor.Trim().ToLowerInvariant();

        if (normalizado.Length > 200)
            throw new ArgumentException("El sitio web no puede superar 200 caracteres.");

        if (!normalizado.StartsWith("http://") && !normalizado.StartsWith("https://"))
            throw new ArgumentException(
                $"El sitio web debe comenzar con http:// o https://. Se recibió: '{valor}'");

        return new SitioWebAircraftManufacturer(normalizado);
    }

    public override string ToString() => Valor;
}