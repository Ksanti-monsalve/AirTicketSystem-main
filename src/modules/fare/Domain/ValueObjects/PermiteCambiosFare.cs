// src/modules/fare/Domain/ValueObjects/PermiteCambiosFare.cs
namespace AirTicketSystem.modules.fare.Domain.ValueObjects;

public sealed class PermiteCambiosFare
{
    public bool Valor { get; }

    private PermiteCambiosFare(bool valor) => Valor = valor;

    public static PermiteCambiosFare Crear(bool valor) => new(valor);

    public static PermiteCambiosFare Permite() => new(true);

    public static PermiteCambiosFare NoPermite() => new(false);

    public override string ToString() => Valor ? "Permite cambios" : "No permite cambios";
}