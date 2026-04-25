// src/modules/documenttype/Domain/ValueObjects/DescripcionDocumentType.cs
namespace AirTicketSystem.modules.documenttype.Domain.ValueObjects;

public sealed class DescripcionDocumentType
{
    public string Valor { get; }

    private DescripcionDocumentType(string valor) => Valor = valor;

    public static DescripcionDocumentType Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción del tipo de documento no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("La descripción debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("La descripción no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("La descripción del tipo de documento no puede contener números.");

        return new DescripcionDocumentType(normalizado);
    }

    public override string ToString() => Valor;
}