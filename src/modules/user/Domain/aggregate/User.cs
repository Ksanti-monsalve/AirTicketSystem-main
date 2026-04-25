// src/modules/user/Domain/aggregate/User.cs
using AirTicketSystem.modules.user.Domain.ValueObjects;

namespace AirTicketSystem.modules.user.Domain.aggregate;

public sealed class User
{
    public int Id { get; private set; }
    public int PersonaId { get; private set; }
    public int RolId { get; private set; }
    public UsernameUser Username { get; private set; } = null!;
    public PasswordHashUser PasswordHash { get; private set; } = null!;
    public ActivoUser Activo { get; private set; } = null!;
    public FechaRegistroUser FechaRegistro { get; private set; } = null!;
    public UltimoLoginUser? UltimoLogin { get; private set; }
    public IntentosFallidosUser IntentosFallidos { get; private set; } = null!;

    private User() { }

    public static User Crear(
        int personaId,
        int rolId,
        string username,
        string passwordHash)
    {
        if (personaId <= 0)
            throw new ArgumentException("La persona es obligatoria.");

        if (rolId <= 0)
            throw new ArgumentException("El rol es obligatorio.");

        return new User
        {
            PersonaId        = personaId,
            RolId            = rolId,
            Username         = UsernameUser.Crear(username),
            PasswordHash     = PasswordHashUser.Crear(passwordHash),
            Activo           = ActivoUser.Activo(),
            FechaRegistro    = FechaRegistroUser.Ahora(),
            UltimoLogin      = null,
            IntentosFallidos = IntentosFallidosUser.Cero()
        };
    }

    public static User Reconstituir(
        int id,
        int personaId,
        int rolId,
        string username,
        string passwordHash,
        bool activo,
        DateTime fechaRegistro,
        DateTime? ultimoLogin,
        int intentosFallidos)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        var user = new User
        {
            PersonaId        = personaId,
            RolId            = rolId,
            Username         = UsernameUser.Crear(username),
            PasswordHash     = PasswordHashUser.Crear(passwordHash),
            Activo           = ActivoUser.Crear(activo),
            FechaRegistro    = FechaRegistroUser.Crear(fechaRegistro),
            UltimoLogin      = ultimoLogin.HasValue
                ? UltimoLoginUser.Crear(ultimoLogin.Value)
                : null,
            IntentosFallidos = IntentosFallidosUser.Crear(intentosFallidos)
        };
        user.Id = id;
        return user;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        Id = id;
    }

    // ── Autenticación ────────────────────────────────────────

    /// <summary>
    /// Registra un intento de login fallido.
    /// Si se alcanza el máximo, la cuenta queda bloqueada.
    /// </summary>
    public void RegistrarIntentoFallido()
    {
        if (!Activo.Valor)
            throw new InvalidOperationException(
                "No se puede registrar intentos en una cuenta inactiva.");

        IntentosFallidos = IntentosFallidos.Incrementar();

        if (IntentosFallidos.CuentaBloqueada)
            Activo = ActivoUser.Inactivo();
    }

    /// <summary>
    /// Registra un login exitoso.
    /// Reinicia los intentos fallidos y actualiza el último login.
    /// </summary>
    public void RegistrarLoginExitoso()
    {
        if (!Activo.Valor)
            throw new InvalidOperationException(
                "No se puede hacer login en una cuenta inactiva.");

        UltimoLogin      = UltimoLoginUser.Ahora();
        IntentosFallidos = IntentosFallidos.Reiniciar();
    }

    // ── Gestión de cuenta ────────────────────────────────────

    public void CambiarPassword(string nuevoPasswordHash)
    {
        PasswordHash     = PasswordHashUser.Crear(nuevoPasswordHash);
        IntentosFallidos = IntentosFallidos.Reiniciar();
    }

    public void CambiarRol(int nuevoRolId)
    {
        if (nuevoRolId <= 0)
            throw new ArgumentException("El nuevo rol es obligatorio.");

        RolId = nuevoRolId;
    }

    public void Activar()
    {
        Activo           = ActivoUser.Activo();
        IntentosFallidos = IntentosFallidos.Reiniciar();
    }

    public void Desactivar()
    {
        Activo = ActivoUser.Inactivo();
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaBloqueado =>
        IntentosFallidos.CuentaBloqueada || !Activo.Valor;

    public bool NuncaHaIngresado =>
        UltimoLogin is null;

    public override string ToString() => Username.ToString();
}