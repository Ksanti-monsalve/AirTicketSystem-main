// src/modules/user/Domain/aggregate/AccessLog.cs
using AirTicketSystem.modules.user.Domain.ValueObjects;

namespace AirTicketSystem.modules.user.Domain.aggregate;

public sealed class AccessLog
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public FechaAccesoAccessLog FechaAcceso { get; private set; } = null!;
    public TipoAccessLog Tipo { get; private set; } = null!;
    public IpAddressAccessLog? IpAddress { get; private set; }

    private AccessLog() { }

    public static AccessLog Crear(
        int usuarioId,
        string tipo,
        string? ipAddress = null)
    {
        if (usuarioId <= 0)
            throw new ArgumentException("El usuario es obligatorio.");

        return new AccessLog
        {
            UsuarioId   = usuarioId,
            FechaAcceso = FechaAccesoAccessLog.Ahora(),
            Tipo        = TipoAccessLog.Crear(tipo),
            IpAddress   = ipAddress is not null
                ? IpAddressAccessLog.Crear(ipAddress)
                : null
        };
    }

    public static AccessLog Reconstituir(
        int id,
        int usuarioId,
        DateTime fechaAcceso,
        string tipo,
        string? ipAddress)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del log de acceso no es válido.");

        var log = new AccessLog
        {
            UsuarioId   = usuarioId,
            FechaAcceso = FechaAccesoAccessLog.Crear(fechaAcceso),
            Tipo        = TipoAccessLog.Crear(tipo),
            IpAddress   = ipAddress is not null
                ? IpAddressAccessLog.Crear(ipAddress)
                : null
        };
        log.Id = id;
        return log;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del log de acceso no es válido.");

        Id = id;
    }

    // Métodos de fábrica expresivos para los tres tipos de evento
    public static AccessLog CrearLogin(int usuarioId, string? ipAddress = null)
        => Crear(usuarioId, "LOGIN", ipAddress);

    public static AccessLog CrearLogout(int usuarioId, string? ipAddress = null)
        => Crear(usuarioId, "LOGOUT", ipAddress);

    public static AccessLog CrearIntentoFallido(int usuarioId, string? ipAddress = null)
        => Crear(usuarioId, "INTENTO_FALLIDO", ipAddress);

    // Propiedades de negocio
    public bool EsEventoDeSeguridad =>
        Tipo.EsIntentoFallido;

    public override string ToString() =>
        $"[{FechaAcceso}] Usuario {UsuarioId} — {Tipo}";
}