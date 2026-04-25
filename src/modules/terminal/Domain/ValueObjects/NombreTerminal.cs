// src/modules/terminal/Domain/ValueObjects/NombreTerminal.cs
namespace AirTicketSystem.modules.terminal.Domain.ValueObjects;

public sealed class NombreTerminal
{
    public string Valor { get; }

    private NombreTerminal(string valor) => Valor = valor;

    public static NombreTerminal Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre de la terminal no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre de la terminal debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("El nombre de la terminal no puede superar 50 caracteres.");

        return new NombreTerminal(normalizado);
    }

    public override string ToString() => Valor;
}