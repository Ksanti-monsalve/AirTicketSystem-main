// src/modules/terminal/Domain/ValueObjects/DescripcionTerminal.cs
namespace AirTicketSystem.modules.terminal.Domain.ValueObjects;

public sealed class DescripcionTerminal
{
    public string Valor { get; }

    private DescripcionTerminal(string valor) => Valor = valor;

    public static DescripcionTerminal Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción de la terminal no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 5)
            throw new ArgumentException("La descripción debe tener al menos 5 caracteres.");

        if (normalizado.Length > 200)
            throw new ArgumentException("La descripción no puede superar 200 caracteres.");

        return new DescripcionTerminal(normalizado);
    }

    public override string ToString() => Valor;
}