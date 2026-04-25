// src/modules/user/Domain/ValueObjects/PasswordHashUser.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class PasswordHashUser
{
    public string Valor { get; }

    private PasswordHashUser(string valor) => Valor = valor;

    /// <summary>
    /// Recibe el hash ya generado externamente (BCrypt, SHA256, etc.).
    /// Este VO no genera el hash — eso es responsabilidad de la infraestructura.
    /// Solo valida que el hash tenga el formato esperado.
    /// </summary>
    public static PasswordHashUser Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El hash de contraseña no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 20)
            throw new ArgumentException("El hash de contraseña no tiene un formato válido.");

        if (normalizado.Length > 255)
            throw new ArgumentException("El hash de contraseña no puede superar 255 caracteres.");

        return new PasswordHashUser(normalizado);
    }

    public override string ToString() => "***";  // Nunca exponer el hash
}