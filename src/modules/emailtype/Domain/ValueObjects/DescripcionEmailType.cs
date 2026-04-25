// src/modules/emailtype/Domain/ValueObjects/DescripcionEmailType.cs
namespace AirTicketSystem.modules.emailtype.Domain.ValueObjects;

public sealed class DescripcionEmailType
{
    public string Valor { get; }

    private DescripcionEmailType(string valor) => Valor = valor;

    public static DescripcionEmailType Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción del tipo de email no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("La descripción debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("La descripción no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("La descripción del tipo de email no puede contener números.");

        return new DescripcionEmailType(normalizado);
    }

    public override string ToString() => Valor;
}