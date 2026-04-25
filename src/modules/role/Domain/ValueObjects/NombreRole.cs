// src/modules/role/Domain/ValueObjects/NombreRole.cs
namespace AirTicketSystem.modules.role.Domain.ValueObjects;

public sealed class NombreRole
{
    public string Valor { get; }

    private NombreRole(string valor) => Valor = valor;

    public static NombreRole Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del rol no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre del rol debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("El nombre del rol no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre del rol no puede contener números.");

        if (!normalizado.All(c => char.IsLetter(c) || c == '_'))
            throw new ArgumentException(
                $"El nombre del rol solo puede contener letras y guión bajo. Se recibió: '{valor}'");

        return new NombreRole(normalizado);
    }

    public override string ToString() => Valor;
}