// src/modules/contactrelationship/Domain/ValueObjects/DescripcionContactRelationship.cs
namespace AirTicketSystem.modules.contactrelationship.Domain.ValueObjects;

public sealed class DescripcionContactRelationship
{
    public string Valor { get; }

    private DescripcionContactRelationship(string valor) => Valor = valor;

    public static DescripcionContactRelationship Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción de la relación de contacto no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("La descripción debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("La descripción no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("La descripción de la relación no puede contener números.");

        return new DescripcionContactRelationship(normalizado);
    }

    public override string ToString() => Valor;
}