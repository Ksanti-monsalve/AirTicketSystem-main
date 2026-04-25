// src/modules/fare/Domain/ValueObjects/PermiteReembolsoFare.cs
namespace AirTicketSystem.modules.fare.Domain.ValueObjects;

public sealed class PermiteReembolsoFare
{
    public bool Valor { get; }

    private PermiteReembolsoFare(bool valor) => Valor = valor;

    public static PermiteReembolsoFare Crear(bool valor) => new(valor);

    public static PermiteReembolsoFare Permite() => new(true);

    public static PermiteReembolsoFare NoPErmite() => new(false);

    public override string ToString() => Valor ? "Permite reembolso" : "No permite reembolso";
}