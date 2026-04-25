// src/modules/client/Domain/ValueObjects/EsPrincipalEmergencyContact.cs
namespace AirTicketSystem.modules.client.Domain.ValueObjects;

public sealed class EsPrincipalEmergencyContact
{
    public bool Valor { get; }

    private EsPrincipalEmergencyContact(bool valor) => Valor = valor;

    public static EsPrincipalEmergencyContact Crear(bool valor) => new(valor);

    public static EsPrincipalEmergencyContact Principal() => new(true);

    public static EsPrincipalEmergencyContact Secundario() => new(false);

    public override string ToString() => Valor ? "Principal" : "Secundario";
}