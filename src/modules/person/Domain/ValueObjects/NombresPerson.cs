// src/modules/person/Domain/ValueObjects/NombresPerson.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class NombresPerson
{
    public string Valor { get; }

    private NombresPerson(string valor) => Valor = valor;

    public static NombresPerson Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre no puede superar 100 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre no puede contener números.");

        if (normalizado.Any(c => c == '@' || c == '#' || c == '$' || c == '%'))
            throw new ArgumentException("El nombre contiene caracteres no permitidos.");

        var capitalizado = string.Join(' ', normalizado
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => char.ToUpper(p[0]) + p[1..].ToLower()));

        return new NombresPerson(capitalizado);
    }

    public override string ToString() => Valor;
}