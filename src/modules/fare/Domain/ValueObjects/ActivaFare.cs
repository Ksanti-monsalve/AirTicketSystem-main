// src/modules/fare/Domain/ValueObjects/ActivaFare.cs
namespace AirTicketSystem.modules.fare.Domain.ValueObjects;

public sealed class ActivaFare
{
    public bool Valor { get; }

    private ActivaFare(bool valor) => Valor = valor;

    public static ActivaFare Crear(bool valor) => new(valor);

    public static ActivaFare Activa() => new(true);

    public static ActivaFare Inactiva() => new(false);

    public override string ToString() => Valor ? "Activa" : "Inactiva";
}