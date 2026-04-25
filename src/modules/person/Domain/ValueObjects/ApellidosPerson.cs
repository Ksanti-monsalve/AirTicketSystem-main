// src/modules/person/Domain/ValueObjects/ApellidosPerson.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class ApellidosPerson
{
    public string Valor { get; }

    private ApellidosPerson(string valor) => Valor = valor;

    public static ApellidosPerson Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Los apellidos no pueden estar vacíos.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("Los apellidos deben tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("Los apellidos no pueden superar 100 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("Los apellidos no pueden contener números.");

        if (normalizado.Any(c => c == '@' || c == '#' || c == '$' || c == '%'))
            throw new ArgumentException("Los apellidos contienen caracteres no permitidos.");

        var capitalizado = string.Join(' ', normalizado
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => char.ToUpper(p[0]) + p[1..].ToLower()));

        return new ApellidosPerson(capitalizado);
    }

    public override string ToString() => Valor;
}