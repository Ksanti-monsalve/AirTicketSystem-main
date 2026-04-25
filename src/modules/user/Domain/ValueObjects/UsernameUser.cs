// src/modules/user/Domain/ValueObjects/UsernameUser.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class UsernameUser
{
    public string Valor { get; }

    private UsernameUser(string valor) => Valor = valor;

    public static UsernameUser Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre de usuario no puede estar vacío.");

        var normalizado = valor.Trim().ToLowerInvariant();

        if (normalizado.Length < 4)
            throw new ArgumentException("El nombre de usuario debe tener al menos 4 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("El nombre de usuario no puede superar 50 caracteres.");

        if (normalizado.Contains(' '))
            throw new ArgumentException("El nombre de usuario no puede contener espacios.");

        // Solo letras, números, puntos, guiones y guiones bajos
        if (!normalizado.All(c => char.IsLetterOrDigit(c) || c == '.' || c == '-' || c == '_'))
            throw new ArgumentException(
                $"El nombre de usuario solo puede contener letras, números, puntos, guiones y guión bajo. Se recibió: '{valor}'");

        // No puede empezar ni terminar con punto, guión o guión bajo
        if (!char.IsLetterOrDigit(normalizado[0]))
            throw new ArgumentException("El nombre de usuario debe comenzar con una letra o número.");

        if (!char.IsLetterOrDigit(normalizado[^1]))
            throw new ArgumentException("El nombre de usuario debe terminar con una letra o número.");

        return new UsernameUser(normalizado);
    }

    public override string ToString() => Valor;
}