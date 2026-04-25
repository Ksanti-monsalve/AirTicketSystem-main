// src/modules/airline/Domain/ValueObjects/ActivaAerolinea.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class ActivaAerolinea
{
    public bool Valor { get; }

    private ActivaAerolinea(bool valor) => Valor = valor;

    public static ActivaAerolinea Crear(bool valor) => new(valor);

    public static ActivaAerolinea Activa() => new(true);
    public static ActivaAerolinea Inactiva() => new(false);

    public override string ToString() => Valor ? "Activa" : "Inactiva";
}