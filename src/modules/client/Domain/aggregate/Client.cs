// src/modules/client/Domain/aggregate/Client.cs
using AirTicketSystem.modules.client.Domain.ValueObjects;

namespace AirTicketSystem.modules.client.Domain.aggregate;

public sealed class Client
{
    public int Id { get; private set; }
    public int PersonaId { get; private set; }
    public int UsuarioId { get; private set; }
    public ActivoClient Activo { get; private set; } = null!;
    public FechaRegistroClient FechaRegistro { get; private set; } = null!;

    private Client() { }

    public static Client Crear(int personaId, int usuarioId)
    {
        if (personaId <= 0)
            throw new ArgumentException("La persona es obligatoria.");

        if (usuarioId <= 0)
            throw new ArgumentException("El usuario es obligatorio.");

        return new Client
        {
            PersonaId    = personaId,
            UsuarioId    = usuarioId,
            Activo       = ActivoClient.Activo(),
            FechaRegistro = FechaRegistroClient.Ahora()
        };
    }

    public static Client Reconstituir(
        int id,
        int personaId,
        int usuarioId,
        bool activo,
        DateTime fechaRegistro)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var client = new Client
        {
            PersonaId     = personaId,
            UsuarioId     = usuarioId,
            Activo        = ActivoClient.Crear(activo),
            FechaRegistro = FechaRegistroClient.Crear(fechaRegistro)
        };
        client.Id = id;
        return client;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        Id = id;
    }

    public void Activar()
    {
        if (Activo.Valor)
            throw new InvalidOperationException(
                "El cliente ya se encuentra activo.");

        Activo = ActivoClient.Activo();
    }

    public void Desactivar()
    {
        if (!Activo.Valor)
            throw new InvalidOperationException(
                "El cliente ya se encuentra inactivo.");

        Activo = ActivoClient.Inactivo();
    }

    // Propiedades de negocio
    public bool EstaActivo => Activo.Valor;

    public int DiasComoCliente
    {
        get
        {
            var hoy = DateTime.UtcNow;
            return (int)(hoy - FechaRegistro.Valor).TotalDays;
        }
    }

    public bool EsClienteNuevo => DiasComoCliente <= 30;

    public override string ToString() =>
        $"Cliente #{Id} — Persona {PersonaId}";
}