// src/modules/person/Domain/ValueObjects/DireccionLinea2PersonAddress.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class DireccionLinea2PersonAddress
{
    public string Valor { get; }

    private DireccionLinea2PersonAddress(string valor) => Valor = valor;

    public static DireccionLinea2PersonAddress Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La dirección línea 2 no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("La dirección línea 2 debe tener al menos 2 caracteres.");

        if (normalizado.Length > 200)
            throw new ArgumentException("La dirección línea 2 no puede superar 200 caracteres.");

        return new DireccionLinea2PersonAddress(normalizado);
    }

    public override string ToString() => Valor;
}