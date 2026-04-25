// src/modules/person/Domain/ValueObjects/DireccionLinea1PersonAddress.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class DireccionLinea1PersonAddress
{
    public string Valor { get; }

    private DireccionLinea1PersonAddress(string valor) => Valor = valor;

    public static DireccionLinea1PersonAddress Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La dirección línea 1 no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 5)
            throw new ArgumentException("La dirección línea 1 debe tener al menos 5 caracteres.");

        if (normalizado.Length > 200)
            throw new ArgumentException("La dirección línea 1 no puede superar 200 caracteres.");

        return new DireccionLinea1PersonAddress(normalizado);
    }

    public override string ToString() => Valor;
}