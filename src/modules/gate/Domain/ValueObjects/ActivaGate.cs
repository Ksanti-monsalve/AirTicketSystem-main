// src/modules/gate/Domain/ValueObjects/ActivaGate.cs
namespace AirTicketSystem.modules.gate.Domain.ValueObjects;

public sealed class ActivaGate
{
    public bool Valor { get; }

    private ActivaGate(bool valor) => Valor = valor;

    public static ActivaGate Crear(bool valor) => new(valor);

    public static ActivaGate Activa() => new(true);

    public static ActivaGate Inactiva() => new(false);

    public override string ToString() => Valor ? "Activa" : "Inactiva";
}