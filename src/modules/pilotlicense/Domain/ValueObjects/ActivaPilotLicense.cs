// src/modules/pilotlicense/Domain/ValueObjects/ActivaPilotLicense.cs
namespace AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;

public sealed class ActivaPilotLicense
{
    public bool Valor { get; }

    private ActivaPilotLicense(bool valor) => Valor = valor;

    public static ActivaPilotLicense Crear(bool valor) => new(valor);

    public static ActivaPilotLicense Activa() => new(true);

    public static ActivaPilotLicense Inactiva() => new(false);

    public override string ToString() => Valor ? "Activa" : "Inactiva";
}