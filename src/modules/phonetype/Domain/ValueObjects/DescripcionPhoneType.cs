// src/modules/phonetype/Domain/ValueObjects/DescripcionPhoneType.cs
namespace AirTicketSystem.modules.phonetype.Domain.ValueObjects;

public sealed class DescripcionPhoneType
{
    public string Valor { get; }

    private DescripcionPhoneType(string valor) => Valor = valor;

    public static DescripcionPhoneType Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción del tipo de teléfono no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("La descripción debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("La descripción no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("La descripción del tipo de teléfono no puede contener números.");

        return new DescripcionPhoneType(normalizado);
    }

    public override string ToString() => Valor;
}