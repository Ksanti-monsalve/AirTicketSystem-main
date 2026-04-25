// src/modules/client/Domain/ValueObjects/ActivoClient.cs
namespace AirTicketSystem.modules.client.Domain.ValueObjects;

public sealed class ActivoClient
{
    public bool Valor { get; }

    private ActivoClient(bool valor) => Valor = valor;

    public static ActivoClient Crear(bool valor) => new(valor);

    public static ActivoClient Activo() => new(true);

    public static ActivoClient Inactivo() => new(false);

    public override string ToString() => Valor ? "Activo" : "Inactivo";
}