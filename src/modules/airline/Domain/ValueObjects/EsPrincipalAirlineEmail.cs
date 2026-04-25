// src/modules/airline/Domain/ValueObjects/EsPrincipalAirlineEmail.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class EsPrincipalAirlineEmail
{
    public bool Valor { get; }

    private EsPrincipalAirlineEmail(bool valor) => Valor = valor;

    public static EsPrincipalAirlineEmail Crear(bool valor) => new(valor);

    public static EsPrincipalAirlineEmail Principal() => new(true);

    public static EsPrincipalAirlineEmail Secundario() => new(false);

    public override string ToString() => Valor ? "Principal" : "Secundario";
}