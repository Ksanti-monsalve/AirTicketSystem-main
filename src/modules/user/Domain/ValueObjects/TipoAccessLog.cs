// src/modules/user/Domain/ValueObjects/TipoAccessLog.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class TipoAccessLog
{
    private static readonly string[] _tiposValidos = { "LOGIN", "LOGOUT", "INTENTO_FALLIDO" };

    public string Valor { get; }

    private TipoAccessLog(string valor) => Valor = valor;

    public static TipoAccessLog Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El tipo de acceso no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_tiposValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El tipo de acceso '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _tiposValidos)}");

        return new TipoAccessLog(normalizado);
    }

    public static TipoAccessLog Login() => new("LOGIN");
    public static TipoAccessLog Logout() => new("LOGOUT");
    public static TipoAccessLog IntentoFallido() => new("INTENTO_FALLIDO");

    public bool EsLogin => Valor == "LOGIN";
    public bool EsLogout => Valor == "LOGOUT";
    public bool EsIntentoFallido => Valor == "INTENTO_FALLIDO";

    public override string ToString() => Valor;
}