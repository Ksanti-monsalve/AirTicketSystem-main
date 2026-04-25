// src/modules/addresstype/Domain/ValueObjects/DescripcionAddressType.cs
namespace AirTicketSystem.modules.addresstype.Domain.ValueObjects;

public sealed class DescripcionAddressType
{
    public string Valor { get; }

    private DescripcionAddressType(string valor) => Valor = valor;

    public static DescripcionAddressType Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción del tipo de dirección no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("La descripción debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("La descripción no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("La descripción del tipo de dirección no puede contener números.");

        return new DescripcionAddressType(normalizado);
    }

    public override string ToString() => Valor;
}