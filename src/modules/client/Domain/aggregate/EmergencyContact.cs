// src/modules/client/Domain/aggregate/EmergencyContact.cs
using AirTicketSystem.modules.client.Domain.ValueObjects;

namespace AirTicketSystem.modules.client.Domain.aggregate;

public sealed class EmergencyContact
{
    public int Id { get; private set; }
    public int ClienteId { get; private set; }
    public int PersonaId { get; private set; }
    public int RelacionId { get; private set; }
    public EsPrincipalEmergencyContact EsPrincipal { get; private set; } = null!;

    private EmergencyContact() { }

    public static EmergencyContact Crear(
        int clienteId,
        int personaId,
        int relacionId,
        bool esPrincipal = false)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El cliente es obligatorio.");

        if (personaId <= 0)
            throw new ArgumentException("La persona del contacto es obligatoria.");

        if (relacionId <= 0)
            throw new ArgumentException("La relación del contacto es obligatoria.");

        if (clienteId == personaId)
            throw new InvalidOperationException(
                "El contacto de emergencia no puede ser la misma persona que el cliente.");

        return new EmergencyContact
        {
            ClienteId   = clienteId,
            PersonaId   = personaId,
            RelacionId  = relacionId,
            EsPrincipal = EsPrincipalEmergencyContact.Crear(esPrincipal)
        };
    }

    public static EmergencyContact Reconstituir(
        int id,
        int clienteId,
        int personaId,
        int relacionId,
        bool esPrincipal)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del contacto de emergencia no es válido.");

        var contact = new EmergencyContact
        {
            ClienteId   = clienteId,
            PersonaId   = personaId,
            RelacionId  = relacionId,
            EsPrincipal = EsPrincipalEmergencyContact.Crear(esPrincipal)
        };
        contact.Id = id;
        return contact;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del contacto de emergencia no es válido.");

        Id = id;
    }

    public void MarcarComoPrincipal()
    {
        if (EsPrincipal.Valor)
            throw new InvalidOperationException(
                "Este contacto ya es el contacto principal.");

        EsPrincipal = EsPrincipalEmergencyContact.Principal();
    }

    public void MarcarComoSecundario()
    {
        if (!EsPrincipal.Valor)
            throw new InvalidOperationException(
                "Este contacto ya es un contacto secundario.");

        EsPrincipal = EsPrincipalEmergencyContact.Secundario();
    }

    public void ActualizarRelacion(int relacionId)
    {
        if (relacionId <= 0)
            throw new ArgumentException("La relación del contacto es obligatoria.");

        RelacionId = relacionId;
    }

    // Propiedades de negocio
    public bool EsContactoPrincipal => EsPrincipal.Valor;

    public override string ToString() =>
        $"Contacto de emergencia — Persona {PersonaId} " +
        $"({(EsPrincipal.Valor ? "Principal" : "Secundario")})";
}