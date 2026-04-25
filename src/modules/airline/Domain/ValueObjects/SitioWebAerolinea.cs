// src/modules/airline/Domain/ValueObjects/SitioWebAerolinea.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class SitioWebAerolinea
{
    public string Valor { get; }

    private SitioWebAerolinea(string valor) => Valor = valor;

    public static SitioWebAerolinea Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El sitio web no puede estar vacío.");

        var normalizado = valor.Trim().ToLowerInvariant();

        if (normalizado.Length > 200)
            throw new ArgumentException("El sitio web no puede superar 200 caracteres.");

        if (!normalizado.StartsWith("http://") && !normalizado.StartsWith("https://"))
            throw new ArgumentException("El sitio web debe comenzar con http:// o https://");

        return new SitioWebAerolinea(normalizado);
    }

    public override string ToString() => Valor;
}