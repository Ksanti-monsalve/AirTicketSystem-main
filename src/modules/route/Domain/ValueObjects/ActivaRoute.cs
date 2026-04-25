// src/modules/route/Domain/ValueObjects/ActivaRoute.cs
namespace AirTicketSystem.modules.route.Domain.ValueObjects;

public sealed class ActivaRoute
{
    public bool Valor { get; }

    private ActivaRoute(bool valor) => Valor = valor;

    public static ActivaRoute Crear(bool valor) => new(valor);

    public static ActivaRoute Activa() => new(true);

    public static ActivaRoute Inactiva() => new(false);

    public override string ToString() => Valor ? "Activa" : "Inactiva";
}