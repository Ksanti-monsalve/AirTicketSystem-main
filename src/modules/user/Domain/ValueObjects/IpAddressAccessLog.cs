// src/modules/user/Domain/ValueObjects/IpAddressAccessLog.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class IpAddressAccessLog
{
    public string Valor { get; }

    private IpAddressAccessLog(string valor) => Valor = valor;

    public static IpAddressAccessLog Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La dirección IP no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length > 45)
            throw new ArgumentException("La dirección IP no puede superar 45 caracteres.");

        // Valida IPv4 o IPv6
        if (!EsIpv4Valida(normalizado) && !EsIpv6Valida(normalizado))
            throw new ArgumentException(
                $"'{valor}' no es una dirección IP válida (IPv4 o IPv6).");

        return new IpAddressAccessLog(normalizado);
    }

    private static bool EsIpv4Valida(string ip)
    {
        var partes = ip.Split('.');
        if (partes.Length != 4) return false;

        return partes.All(p =>
            int.TryParse(p, out var num) && num >= 0 && num <= 255);
    }

    private static bool EsIpv6Valida(string ip)
    {
        // Validación básica de IPv6: grupos separados por ':'
        var partes = ip.Split(':');
        if (partes.Length < 2 || partes.Length > 8) return false;

        return partes.All(p =>
            p.Length <= 4 && p.All(c => char.IsAsciiHexDigit(c)));
    }

    public bool EsIpv4 => Valor.Contains('.');
    public bool EsIpv6 => Valor.Contains(':');

    public override string ToString() => Valor;
}